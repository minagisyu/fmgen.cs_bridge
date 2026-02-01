namespace fmgen.cs_bridge;

/// <summary>
/// OPNA (YM2608) FM 音源チップのラッパークラス
/// </summary>
/// <remarks>
/// Mix メソッドは「加算合成」を行います。呼び出し前にバッファをゼロクリアしてください
/// </remarks>
public sealed class OPNA : IDisposable
{
	readonly NativeLibrary.OPNASafeHandle _handle;
	bool _disposed;

	/// <summary>OPNA インスタンスを生成します</summary>
	public OPNA()
	{
		var ptr = NativeLibrary.OPNA_Create();
		if (ptr == 0) throw new InvalidOperationException("OPNA_Create failed.");
		_handle = new NativeLibrary.OPNASafeHandle(ptr, ownsHandle: true);
	}

	/// <summary>OPNA を初期化します</summary>
	/// <param name="clock">クロック周波数 (Hz)</param>
	/// <param name="rate">サンプリングレート (Hz)</param>
	/// <param name="interpolation">補間フラグ (0 or 1)</param>
	/// <param name="rhythmpath">リズムサンプルディレクトリパス (UTF-8)。null 可</param>
	/// <returns>成功時は true</returns>
	public bool Init(uint clock, uint rate, int interpolation = 0, string? rhythmpath = null)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_Init(_handle, clock, rate, interpolation, rhythmpath) != 0;
	}

	/// <summary>リズムサンプルをロードします</summary>
	/// <returns>成功時は true</returns>
	public bool LoadRhythmSample(string? rhythmpath)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_LoadRhythmSample(_handle, rhythmpath) != 0;
	}

	/// <summary>サンプリングレートを変更します</summary>
	public bool SetRate(uint clock, uint rate, int interpolation = 0)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_SetRate(_handle, clock, rate, interpolation) != 0;
	}

	/// <summary>OPNA をリセットします</summary>
	public void Reset()
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_Reset(_handle);
	}

	/// <summary>レジスタに値を書き込みます</summary>
	public void SetReg(uint addr, uint data)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetReg(_handle, addr, data);
	}

	/// <summary>レジスタの値を取得します</summary>
	public uint GetReg(uint addr)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_GetReg(_handle, addr);
	}

	/// <summary>ステータスを読み取ります</summary>
	public uint ReadStatus()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_ReadStatus(_handle);
	}

	/// <summary>拡張ステータスを読み取ります</summary>
	public uint ReadStatusEx()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_ReadStatusEx(_handle);
	}

	/// <summary>チャンネルマスクを設定します</summary>
	public void SetChannelMask(uint mask)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetChannelMask(_handle, mask);
	}

	/// <summary>FM 音量を設定します (単位: dB)</summary>
	public void SetVolumeFM(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetVolumeFM(_handle, db);
	}

	/// <summary>PSG 音量を設定します (単位: dB)</summary>
	public void SetVolumePSG(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetVolumePSG(_handle, db);
	}

	/// <summary>ADPCM 音量を設定します (単位: dB)</summary>
	public void SetVolumeADPCM(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetVolumeADPCM(_handle, db);
	}

	/// <summary>リズム全体の音量を設定します (単位: dB)</summary>
	public void SetVolumeRhythmTotal(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetVolumeRhythmTotal(_handle, db);
	}

	/// <summary>リズム個別チャンネルの音量を設定します (単位: dB)</summary>
	public void SetVolumeRhythm(int index, int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPNA_SetVolumeRhythm(_handle, index, db);
	}

	/// <summary>
	/// 内部 ADPCM バッファへのポインタを返します（256KB 固定）
	/// </summary>
	/// <remarks>
	/// 危険なポインタ操作です。この領域はネイティブ側が所有しています
	/// </remarks>
	public unsafe byte* GetADPCMBuffer()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_GetADPCMBuffer(_handle);
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
				NativeLibrary.OPNA_Mix(_handle, p, nsamples);
			}
		}
	}

	/// <summary>タイマーを進め、マイクロ秒単位でカウントします</summary>
	public int Count(int us)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_Count(_handle, us);
	}

	/// <summary>次のタイマーイベントまでのサンプル数を取得します</summary>
	public int GetNextEvent()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPNA_GetNextEvent(_handle);
	}

	void ThrowIfDisposed()
		=> ObjectDisposedException.ThrowIf(_disposed, this);

	/// <inheritdoc/>
	public void Dispose()
	{
		if (_disposed) return;
		_disposed = true;
		_handle.Dispose();
	}
}
