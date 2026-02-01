using fmgen.cs_bridge;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace fmgen.cs_bridge_test;

[TestClass]
public sealed class BridgeSmokeTests
{
    [TestMethod]
    public void Chips_CreateInitMixDispose_Succeeds()
    {
        int[] buffer = new int[64];

        // PSG
        using (var psg = new PSG())
        {
            psg.SetClock(3579545, 44100);
            psg.Reset();
            psg.SetReg(0, 0);
            Assert.AreEqual((uint)0, psg.GetReg(0));
            psg.SetVolume(0);
            psg.SetChannelMask(0);
            Array.Clear(buffer, 0, buffer.Length);
            psg.Mix(buffer, 1);
        }

        // OPN
        using (var opn = new OPN())
        {
            Assert.IsTrue(opn.Init(3579545, 44100));
            Assert.IsTrue(opn.SetRate(3579545, 44100));
            opn.Reset();
            opn.SetReg(0, 0);
            Assert.AreEqual((uint)0, opn.GetReg(0));
            _ = opn.ReadStatus();
            opn.SetChannelMask(0);
            opn.SetVolumeFM(0);
            opn.SetVolumePSG(0);
            Array.Clear(buffer, 0, buffer.Length);
            opn.Mix(buffer, 1);
            _ = opn.Count(1000);
            _ = opn.GetNextEvent();
        }

        // OPNA
        using (var opna = new OPNA())
        {
            Assert.IsTrue(opna.Init(8000000, 44100));
            Assert.IsTrue(opna.SetRate(8000000, 44100));
            opna.Reset();
            opna.SetReg(0, 0);
            Assert.AreEqual((uint)0, opna.GetReg(0));
            _ = opna.ReadStatus();
            _ = opna.ReadStatusEx();
            opna.SetChannelMask(0);
            opna.SetVolumeFM(0);
            opna.SetVolumePSG(0);
            opna.SetVolumeADPCM(0);
            opna.SetVolumeRhythmTotal(0);
            opna.SetVolumeRhythm(0, 0);

            unsafe
            {
                var p = opna.GetADPCMBuffer();
                Assert.AreNotEqual((nint)0, (nint)p);
            }

            Array.Clear(buffer, 0, buffer.Length);
            opna.Mix(buffer, 1);
            _ = opna.Count(1000);
            _ = opna.GetNextEvent();
        }

        // OPNB
        using (var opnb = new OPNB())
        {
            byte[] dummyAdpcmA = [0];
            byte[] dummyAdpcmB = new byte[256];

            Assert.IsTrue(opnb.Init(8000000, 44100, 0, dummyAdpcmA, dummyAdpcmB));
            Assert.IsTrue(opnb.SetRate(8000000, 44100));
            opnb.Reset();
            opnb.SetReg(0, 0);
            Assert.AreEqual((uint)0, opnb.GetReg(0));
            _ = opnb.ReadStatus();
            _ = opnb.ReadStatusEx();
            opnb.SetChannelMask(0);
            opnb.SetVolumeFM(0);
            opnb.SetVolumePSG(0);
            opnb.SetVolumeADPCMATotal(0);
            opnb.SetVolumeADPCMA(0, 0);
            opnb.SetVolumeADPCMB(0);
            Array.Clear(buffer, 0, buffer.Length);
            opnb.Mix(buffer, 1);
            _ = opnb.Count(1000);
            _ = opnb.GetNextEvent();
        }

        // OPM
        using (var opm = new OPM())
        {
            Assert.IsTrue(opm.Init(3579545, 44100));
            Assert.IsTrue(opm.SetRate(3579545, 44100));
            opm.Reset();
            opm.SetReg(0, 0);
            _ = opm.ReadStatus();
            opm.SetChannelMask(0);
            opm.SetVolume(0);
            Array.Clear(buffer, 0, buffer.Length);
            opm.Mix(buffer, 1);
            _ = opm.Count(1000);
            _ = opm.GetNextEvent();
        }

        Assert.IsTrue(true);
    }

    [TestMethod]
    public void OPNB_InitWithGC_BufferRemainsPinned()
    {
        // ADPCM バッファがピン止めされているため、GC 後も Mix が安全に動作することを確認
        using var opnb = new OPNB();

        // 一時変数として ADPCM バッファを作成し、Init に渡す
        byte[] adpcmA = new byte[1024];
        byte[] adpcmB = new byte[2048];
        for (int i = 0; i < adpcmA.Length; i++) adpcmA[i] = (byte)(i & 0xFF);
        for (int i = 0; i < adpcmB.Length; i++) adpcmB[i] = (byte)(i & 0xFF);

        Assert.IsTrue(opnb.Init(8000000, 44100, 0, adpcmA, adpcmB));

        // 元の配列参照を破棄して GC を強制実行
        adpcmA = null!;
        adpcmB = null!;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);

        // GC 後も Mix がクラッシュせずに動作すること
        int[] buffer = new int[64];
        opnb.Mix(buffer, 1);

        // Dispose パスも通す（using で自動的に呼ばれる）
    }
}
