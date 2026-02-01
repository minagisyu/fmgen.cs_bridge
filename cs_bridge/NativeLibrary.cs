using Microsoft.Win32.SafeHandles;

namespace fmgen.cs_bridge;

/// <summary>
/// fmgen ネイティブライブラリ (fmgen.dll / libfmgen.so / libfmgen.dylib) への P/Invoke 定義
/// bridge/fmgen_c.h のすべての関数を一括定義する
/// </summary>
/// <remarks>
/// チップ別の P/Invoke 定義は partial class として以下のファイルに分割されている:
/// <list type="bullet">
/// <item><description>NativeLibrary.PSG.cs - PSG (AY-3-8910)</description></item>
/// <item><description>NativeLibrary.OPN.cs - OPN (YM2203)</description></item>
/// <item><description>NativeLibrary.OPNA.cs - OPNA (YM2608)</description></item>
/// <item><description>NativeLibrary.OPNB.cs - OPNB (YM2610/B)</description></item>
/// <item><description>NativeLibrary.OPM.cs - OPM (YM2151)</description></item>
/// </list>
/// </remarks>
static partial class NativeLibrary
{
	const string LibraryName = "fmgen";

	/// <summary>OPM ネイティブハンドル用の SafeHandle</summary>
	public sealed class OPMSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public OPMSafeHandle() : base(ownsHandle: true) { }
		public OPMSafeHandle(nint existingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(existingHandle);
		protected override bool ReleaseHandle() { OPM_Destroy(handle); return true; }
	}

	/// <summary>OPN ネイティブハンドル用の SafeHandle</summary>
	public sealed class OPNSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public OPNSafeHandle() : base(ownsHandle: true) { }
		public OPNSafeHandle(nint existingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(existingHandle);
		protected override bool ReleaseHandle() { OPN_Destroy(handle); return true; }
	}

	/// <summary>OPNA ネイティブハンドル用の SafeHandle</summary>
	public sealed class OPNASafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public OPNASafeHandle() : base(ownsHandle: true) { }
		public OPNASafeHandle(nint existingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(existingHandle);
		protected override bool ReleaseHandle() { OPNA_Destroy(handle); return true; }
	}

	public sealed class OPNBSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public OPNBSafeHandle() : base(ownsHandle: true) { }
		public OPNBSafeHandle(nint existingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(existingHandle);
		protected override bool ReleaseHandle() { OPNB_Destroy(handle); return true; }
	}

	/// <summary>PSG ネイティブハンドル用の SafeHandle</summary>
	public sealed class PSGSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public PSGSafeHandle() : base(ownsHandle: true) { }
		public PSGSafeHandle(nint existingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(existingHandle);
		protected override bool ReleaseHandle() { PSG_Destroy(handle); return true; }
	}

}
