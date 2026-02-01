using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

/// <summary>
/// OPNB (YM2610/B) FM 音源チップのラッパークラス
/// </summary>
/// <remarks>
/// <para>Mix メソッドは「加算合成」を行います。呼び出し前にバッファをゼロクリアしてください</para>
/// <para>Init に渡す ADPCMA/ADPCMB バッファは内部にコピーされ、インスタンスの寿命中ピン止めされます</para>
/// </remarks>
public sealed class OPNB : IDisposable
{
	readonly NativeLibrary.OPNBSafeHandle _handle;
	bool _disposed;

	// ADPCM バッファ（内部コピー）とピン止めハンドル
	byte[]? _adpcmaBuffer;
	byte[]? _adpcmbBuffer;
	GCHandle _adpcmaHandle;
	GCHandle _adpcmbHandle;

	/// <summary>OPNB インスタンスを生成します</summary>
	public OPNB()
	{
		var ptr = NativeLibrary.OPNB_Create();
		if (ptr == 0) throw new InvalidOperationException("OPNB_Create failed.");
		_handle = new NativeLibrary.OPNBSafeHandle(ptr, ownsHandle: true);
	}

	/// <summary>OPNB を初期化します</summary>
	/// <param name="clock">クロック周波数 (Hz)</param>
	/// <param name="rate">サンプリングレート (Hz)</param>
	/// <param name="interpolation">補間フラグ (0 or 1)</param>
	/// <param name="adpcma">ADPCMA サンプルバッファ（内部にコピーされます）</param>
	/// <param name="adpcmb">ADPCMB サンプルバッファ（内部にコピーされます）</param>
	/// <returns>成功時は true。</returns>
	/// <remarks>
	/// 複数回呼び出した場合、既存のバッファは解放されて新しいバッファに置き換わります。
	/// </remarks>
	public unsafe bool Init(uint clock, uint rate, int interpolation,
		ReadOnlySpan<byte> adpcma, ReadOnlySpan<byte> adpcmb)
	{
		ThrowIfDisposed();

		// 既存のピン止めを解放（再初期化対応）
		FreeAdpcmHandles();

		// バッファをコピーしてピン止め
		_adpcmaBuffer = adpcma.ToArray();
		_adpcmbBuffer = adpcmb.ToArray();
		_adpcmaHandle = GCHandle.Alloc(_adpcmaBuffer, GCHandleType.Pinned);
		_adpcmbHandle = GCHandle.Alloc(_adpcmbBuffer, GCHandleType.Pinned);

		var pa = (byte*)_adpcmaHandle.AddrOfPinnedObject();
		var pb = (byte*)_adpcmbHandle.AddrOfPinnedObject();

		return NativeLibrary.OPNB_Init(_handle, clock, rate, interpolation,
			pa, _adpcmaBuffer.Length, pb, _adpcmbBuffer.Length) != 0;
	}

	/// <summary>ADPCM ピン止めハンドルを解放します</summary>
	void FreeAdpcmHandles()
	{
		if (_adpcmaHandle.IsAllocated)
		{
			_adpcmaHandle.Free();
		}
		if (_adpcmbHandle.IsAllocated)
		{
			_adpcmbHandle.Free();
		}
		_adpcmaBuffer = null;
		_adpcmbBuffer = null;
	}

	/// <summary>サンプリングレートを変更します</summary>
	public bool SetRate(uint clock, uint rate, int interpolation = 0)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_SetRate(_handle, clock, rate, interpolation) != 0;
	}

	/// <summary>OPNB をリセットします</summary>
	public void Reset()
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_Reset(_handle);
	}

	/// <summary>レジスタに値を書き込みます</summary>
	public void SetReg(uint addr, uint data)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetReg(_handle, addr, data);
	}

	/// <summary>レジスタの値を取得します</summary>
	public uint GetReg(uint addr)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_GetReg(_handle, addr);
	}

	/// <summary>ステータスを読み取ります</summary>
	public uint ReadStatus()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_ReadStatus(_handle);
	}

	/// <summary>拡張ステータスを読み取ります</summary>
	public uint ReadStatusEx()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_ReadStatusEx(_handle);
	}

	/// <summary>チャンネルマスクを設定します</summary>
	public void SetChannelMask(uint mask)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetChannelMask(_handle, mask);
	}

	/// <summary>FM 音量を設定します (単位: dB)</summary>
	public void SetVolumeFM(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetVolumeFM(_handle, db);
	}

	/// <summary>PSG 音量を設定します (単位: dB)</summary>
	public void SetVolumePSG(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetVolumePSG(_handle, db);
	}

	/// <summary>ADPCMA 全体の音量を設定します (単位: dB)</summary>
	public void SetVolumeADPCMATotal(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetVolumeADPCMATotal(_handle, db);
	}

	/// <summary>ADPCMA 個別チャンネルの音量を設定します (単位: dB)</summary>
	public void SetVolumeADPCMA(int index, int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetVolumeADPCMA(_handle, index, db);
	}

	/// <summary>ADPCMB 音量を設定します (単位: dB)</summary>
	public void SetVolumeADPCMB(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNB_SetVolumeADPCMB(_handle, db);
	}

	/// <summary>
	/// PCM を nsamples フレーム分合成し、dest に加算します（ステレオ interleaved: L,R,L,R,...）
	/// </summary>
	public void Mix(Span<int> dest, int nsamples)
	{
		ThrowIfDisposed();
		if (dest.Length < nsamples * 2)
			throw new ArgumentException("Buffer too small.", nameof(dest));

		unsafe
		{
			fixed (int* p = dest)
			{
				NativeLibrary.OPNB_Mix(_handle, p, nsamples);
			}
		}
	}

	/// <summary>タイマーを進め、マイクロ秒単位でカウントします</summary>
	public int Count(int us)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_Count(_handle, us);
	}

	/// <summary>次のタイマーイベントまでのサンプル数を取得します</summary>
	public int GetNextEvent()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNB_GetNextEvent(_handle);
	}

	void ThrowIfDisposed()
		=> ObjectDisposedException.ThrowIf(_disposed, this);

	/// <inheritdoc/>
	public void Dispose()
	{
		if (_disposed) return;
		_disposed = true;
		FreeAdpcmHandles();
		_handle.Dispose();
	}
}
