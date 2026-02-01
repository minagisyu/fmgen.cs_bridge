using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

static partial class NativeLibrary
{
	// ========================================
	// OPNA (YM2608)
	// ========================================

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Create")]
	public static partial nint OPNA_Create();

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Destroy")]
	public static partial void OPNA_Destroy(nint opna);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Init", StringMarshalling = StringMarshalling.Utf8)]
	public static partial int OPNA_Init(OPNASafeHandle opna, uint clock, uint rate, int interpolation, string? rhythmpath);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_LoadRhythmSample", StringMarshalling = StringMarshalling.Utf8)]
	public static partial int OPNA_LoadRhythmSample(OPNASafeHandle opna, string? rhythmpath);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetRate")]
	public static partial int OPNA_SetRate(OPNASafeHandle opna, uint clock, uint rate, int interpolation);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Reset")]
	public static partial void OPNA_Reset(OPNASafeHandle opna);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetReg")]
	public static partial void OPNA_SetReg(OPNASafeHandle opna, uint addr, uint data);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_GetReg")]
	public static partial uint OPNA_GetReg(OPNASafeHandle opna, uint addr);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_ReadStatus")]
	public static partial uint OPNA_ReadStatus(OPNASafeHandle opna);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_ReadStatusEx")]
	public static partial uint OPNA_ReadStatusEx(OPNASafeHandle opna);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetChannelMask")]
	public static partial void OPNA_SetChannelMask(OPNASafeHandle opna, uint mask);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetVolumeFM")]
	public static partial void OPNA_SetVolumeFM(OPNASafeHandle opna, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetVolumePSG")]
	public static partial void OPNA_SetVolumePSG(OPNASafeHandle opna, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetVolumeADPCM")]
	public static partial void OPNA_SetVolumeADPCM(OPNASafeHandle opna, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetVolumeRhythmTotal")]
	public static partial void OPNA_SetVolumeRhythmTotal(OPNASafeHandle opna, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_SetVolumeRhythm")]
	public static partial void OPNA_SetVolumeRhythm(OPNASafeHandle opna, int index, int db);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_GetADPCMBuffer")]
	public static unsafe partial byte* OPNA_GetADPCMBuffer(OPNASafeHandle opna);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Mix")]
	public static unsafe partial void OPNA_Mix(OPNASafeHandle opna, int* dest, int nsamples);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_Count")]
	public static partial int OPNA_Count(OPNASafeHandle opna, int us);

	[LibraryImport(LibraryName, EntryPoint = "OPNA_GetNextEvent")]
	public static partial int OPNA_GetNextEvent(OPNASafeHandle opna);
}
