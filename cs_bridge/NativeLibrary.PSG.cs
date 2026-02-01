using System.Runtime.InteropServices;

namespace fmgen.cs_bridge;

static partial class NativeLibrary
{
	// ========================================
	// PSG (AY-3-8910 互換)
	// ========================================

	[LibraryImport(LibraryName, EntryPoint = "PSG_Create")]
	public static partial nint PSG_Create();

	[LibraryImport(LibraryName, EntryPoint = "PSG_Destroy")]
	public static partial void PSG_Destroy(nint psg);

	[LibraryImport(LibraryName, EntryPoint = "PSG_Reset")]
	public static partial void PSG_Reset(PSGSafeHandle psg);

	[LibraryImport(LibraryName, EntryPoint = "PSG_SetClock")]
	public static partial void PSG_SetClock(PSGSafeHandle psg, int clock, int rate);

	[LibraryImport(LibraryName, EntryPoint = "PSG_SetReg")]
	public static partial void PSG_SetReg(PSGSafeHandle psg, uint regnum, byte data);

	[LibraryImport(LibraryName, EntryPoint = "PSG_GetReg")]
	public static partial uint PSG_GetReg(PSGSafeHandle psg, uint regnum);

	[LibraryImport(LibraryName, EntryPoint = "PSG_SetVolume")]
	public static partial void PSG_SetVolume(PSGSafeHandle psg, int vol);

	[LibraryImport(LibraryName, EntryPoint = "PSG_SetChannelMask")]
	public static partial void PSG_SetChannelMask(PSGSafeHandle psg, int mask);

	[LibraryImport(LibraryName, EntryPoint = "PSG_Mix")]
	public static unsafe partial void PSG_Mix(PSGSafeHandle psg, int* dest, int nsamples);
}
