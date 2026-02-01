namespace fmgen.cs_bridge;

/// <summary>
/// OPM (YM2151) FM 音源チップのラッパークラス
/// </summary>
/// <remarks>
/// Mix メソッドは「加算合成」を行います。呼び出し前にバッファをゼロクリアしてください
/// </remarks>
public sealed class OPM : IDisposable
{
	readonly NativeLibrary.OPMSafeHandle _handle;
	bool _disposed;

	/// <summary>OPM インスタンスを生成します。</summary>
	public OPM()
	{
		var ptr = NativeLibrary.OPM_Create();
		if (ptr == 0) throw new InvalidOperationException("OPM_Create failed.");
		_handle = new NativeLibrary.OPMSafeHandle(ptr, ownsHandle: true);
	}

	/// <summary>OPM を初期化します</summary>
	/// <param name="clock">クロック周波数 (Hz)</param>
	/// <param name="rate">サンプリングレート (Hz)</param>
	/// <param name="interpolation">補間フラグ (0 or 1)</param>
	/// <returns>成功時は true。</returns>
	public bool Init(uint clock, uint rate, int interpolation = 0)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPM_Init(_handle, clock, rate, interpolation) != 0;
	}

	/// <summary>サンプリングレートを変更します</summary>
	public bool SetRate(uint clock, uint rate, int interpolation = 0)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPM_SetRate(_handle, clock, rate, interpolation) != 0;
	}

	/// <summary>OPM をリセットします</summary>
	public void Reset()
	{
		ThrowIfDisposed();
		NativeLibrary.OPM_Reset(_handle);
	}

	/// <summary>レジスタに値を書き込みます</summary>
	public void SetReg(uint addr, uint data)
	{
		ThrowIfDisposed();
		NativeLibrary.OPM_SetReg(_handle, addr, data);
	}

	/// <summary>ステータスを読み取ります</summary>
	public uint ReadStatus()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPM_ReadStatus(_handle);
	}

	/// <summary>チャンネルマスクを設定します</summary>
	public void SetChannelMask(uint mask)
	{
		ThrowIfDisposed();
		NativeLibrary.OPM_SetChannelMask(_handle, mask);
	}

	/// <summary>音量を設定します (単位: dB)</summary>
	public void SetVolume(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPM_SetVolume(_handle, db);
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
				NativeLibrary.OPM_Mix(_handle, p, nsamples);
			}
		}
	}

	/// <summary>タイマーを進め、マイクロ秒単位でカウントします</summary>
	public int Count(int us)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPM_Count(_handle, us);
	}

	/// <summary>次のタイマーイベントまでのサンプル数を取得します</summary>
	public int GetNextEvent()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPM_GetNextEvent(_handle);
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
