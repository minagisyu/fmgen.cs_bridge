using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

static partial class NativeLibrary
{
	// ========================================
	// OPN (YM2203)
	// ========================================

	[LibraryImport(LibraryName, EntryPoint = "OPN_Create")]
	public static partial nint OPN_Create();

	[LibraryImport(LibraryName, EntryPoint = "OPN_Destroy")]
	public static partial void OPN_Destroy(nint opn);

	[LibraryImport(LibraryName, EntryPoint = "OPN_Init", StringMarshalling = StringMarshalling.Utf8)]
	public static partial int OPN_Init(OPNSafeHandle opn, uint clock, uint rate, int interpolation, string? rhythmpath);

	[LibraryImport(LibraryName, EntryPoint = "OPN_SetRate")]
	public static partial int OPN_SetRate(OPNSafeHandle opn, uint clock, uint rate, int interpolation);

	[LibraryImport(LibraryName, EntryPoint = "OPN_Reset")]
	public static partial void OPN_Reset(OPNSafeHandle opn);

	[LibraryImport(LibraryName, EntryPoint = "OPN_SetReg")]
	public static partial void OPN_SetReg(OPNSafeHandle opn, uint addr, uint data);

	[LibraryImport(LibraryName, EntryPoint = "OPN_GetReg")]
	public static partial uint OPN_GetReg(OPNSafeHandle opn, uint addr);

	[LibraryImport(LibraryName, EntryPoint = "OPN_ReadStatus")]
	public static partial uint OPN_ReadStatus(OPNSafeHandle opn);

	[LibraryImport(LibraryName, EntryPoint = "OPN_SetChannelMask")]
	public static partial void OPN_SetChannelMask(OPNSafeHandle opn, uint mask);

	[LibraryImport(LibraryName, EntryPoint = "OPN_SetVolumeFM")]
	public static partial void OPN_SetVolumeFM(OPNSafeHandle opn, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPN_SetVolumePSG")]
	public static partial void OPN_SetVolumePSG(OPNSafeHandle opn, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPN_Mix")]
	public static unsafe partial void OPN_Mix(OPNSafeHandle opn, int* dest, int nsamples);

	[LibraryImport(LibraryName, EntryPoint = "OPN_Count")]
	public static partial int OPN_Count(OPNSafeHandle opn, int us);

	[LibraryImport(LibraryName, EntryPoint = "OPN_GetNextEvent")]
	public static partial int OPN_GetNextEvent(OPNSafeHandle opn);
}
