using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

static partial class NativeLibrary
{
	// ========================================
	// OPM (YM2151)
	// ========================================

	[LibraryImport(LibraryName, EntryPoint = "OPM_Create")]
	public static partial nint OPM_Create();

	[LibraryImport(LibraryName, EntryPoint = "OPM_Destroy")]
	public static partial void OPM_Destroy(nint opm);

	[LibraryImport(LibraryName, EntryPoint = "OPM_Init")]
	public static partial int OPM_Init(OPMSafeHandle opm, uint clock, uint rate, int interpolation);

	[LibraryImport(LibraryName, EntryPoint = "OPM_SetRate")]
	public static partial int OPM_SetRate(OPMSafeHandle opm, uint clock, uint rate, int interpolation);

	[LibraryImport(LibraryName, EntryPoint = "OPM_Reset")]
	public static partial void OPM_Reset(OPMSafeHandle opm);

	[LibraryImport(LibraryName, EntryPoint = "OPM_SetReg")]
	public static partial void OPM_SetReg(OPMSafeHandle opm, uint addr, uint data);

	[LibraryImport(LibraryName, EntryPoint = "OPM_ReadStatus")]
	public static partial uint OPM_ReadStatus(OPMSafeHandle opm);

	[LibraryImport(LibraryName, EntryPoint = "OPM_SetChannelMask")]
	public static partial void OPM_SetChannelMask(OPMSafeHandle opm, uint mask);

	[LibraryImport(LibraryName, EntryPoint = "OPM_SetVolume")]
	public static partial void OPM_SetVolume(OPMSafeHandle opm, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPM_Mix")]
	public static unsafe partial void OPM_Mix(OPMSafeHandle opm, int* dest, int nsamples);

	[LibraryImport(LibraryName, EntryPoint = "OPM_Count")]
	public static partial int OPM_Count(OPMSafeHandle opm, int us);

	[LibraryImport(LibraryName, EntryPoint = "OPM_GetNextEvent")]
	public static partial int OPM_GetNextEvent(OPMSafeHandle opm);
}
