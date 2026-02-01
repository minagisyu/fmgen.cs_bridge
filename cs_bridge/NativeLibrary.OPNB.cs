using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

static partial class NativeLibrary
{
	/// <summary>OPNB ネイティブハンドル用の SafeHandle</summary>
	// ========================================
	// OPNB (YM2610/B)
	// ========================================

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Create")]
	public static partial nint OPNB_Create();

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Destroy")]
	public static partial void OPNB_Destroy(nint opnb);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Init")]
	public static unsafe partial int OPNB_Init(OPNBSafeHandle opnb, uint clock, uint rate, int interpolation,
		byte* adpcma, int adpcma_size, byte* adpcmb, int adpcmb_size);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetRate")]
	public static partial int OPNB_SetRate(OPNBSafeHandle opnb, uint clock, uint rate, int interpolation);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Reset")]
	public static partial void OPNB_Reset(OPNBSafeHandle opnb);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetReg")]
	public static partial void OPNB_SetReg(OPNBSafeHandle opnb, uint addr, uint data);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_GetReg")]
	public static partial uint OPNB_GetReg(OPNBSafeHandle opnb, uint addr);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_ReadStatus")]
	public static partial uint OPNB_ReadStatus(OPNBSafeHandle opnb);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_ReadStatusEx")]
	public static partial uint OPNB_ReadStatusEx(OPNBSafeHandle opnb);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetChannelMask")]
	public static partial void OPNB_SetChannelMask(OPNBSafeHandle opnb, uint mask);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetVolumeFM")]
	public static partial void OPNB_SetVolumeFM(OPNBSafeHandle opnb, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetVolumePSG")]
	public static partial void OPNB_SetVolumePSG(OPNBSafeHandle opnb, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetVolumeADPCMATotal")]
	public static partial void OPNB_SetVolumeADPCMATotal(OPNBSafeHandle opnb, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetVolumeADPCMA")]
	public static partial void OPNB_SetVolumeADPCMA(OPNBSafeHandle opnb, int index, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_SetVolumeADPCMB")]
	public static partial void OPNB_SetVolumeADPCMB(OPNBSafeHandle opnb, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Mix")]
	public static unsafe partial void OPNB_Mix(OPNBSafeHandle opnb, int* dest, int nsamples);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_Count")]
	public static partial int OPNB_Count(OPNBSafeHandle opnb, int us);

	[LibraryImport(LibraryName, EntryPoint = "OPNB_GetNextEvent")]
	public static partial int OPNB_GetNextEvent(OPNBSafeHandle opnb);
}
