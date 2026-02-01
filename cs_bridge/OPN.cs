namespace fmgen.cs_bridge;

/// <summary>
/// OPN (YM2203) FM 音源チップのラッパークラス
/// </summary>
/// <remarks>
/// Mix メソッドは「加算合成」を行います。呼び出し前にバッファをゼロクリアしてください
/// </remarks>
public sealed class OPN : IDisposable
{
	readonly NativeLibrary.OPNSafeHandle _handle;
	bool _disposed;

	/// <summary>OPN インスタンスを生成します</summary>
	public OPN()
	{
		var ptr = NativeLibrary.OPN_Create();
		if (ptr == 0) throw new InvalidOperationException("OPN_Create failed.");
		_handle = new NativeLibrary.OPNSafeHandle(ptr, ownsHandle: true);
	}

	/// <summary>OPN を初期化します</summary>
	/// <param name="clock">クロック周波数 (Hz)</param>
	/// <param name="rate">サンプリングレート (Hz)</param>
	/// <param name="interpolation">補間フラグ (0 or 1)</param>
	/// <param name="rhythmpath">リズムサンプルパス (UTF-8)。null 可</param>
	/// <returns>成功時は true</returns>
	public bool Init(uint clock, uint rate, int interpolation = 0, string? rhythmpath = null)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_Init(_handle, clock, rate, interpolation, rhythmpath) != 0;
	}

	/// <summary>サンプリングレートを変更します</summary>
	public bool SetRate(uint clock, uint rate, int interpolation = 0)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_SetRate(_handle, clock, rate, interpolation) != 0;
	}

	/// <summary>OPN をリセットします</summary>
	public void Reset()
	{
		ThrowIfDisposed();
		NativeLibrary.OPN_Reset(_handle);
	}

	/// <summary>レジスタに値を書き込みます</summary>
	public void SetReg(uint addr, uint data)
	{
		ThrowIfDisposed();
		NativeLibrary.OPN_SetReg(_handle, addr, data);
	}

	/// <summary>レジスタの値を取得します</summary>
	public uint GetReg(uint addr)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_GetReg(_handle, addr);
	}

	/// <summary>ステータスを読み取ります</summary>
	public uint ReadStatus()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_ReadStatus(_handle);
	}

	/// <summary>チャンネルマスクを設定します</summary>
	public void SetChannelMask(uint mask)
	{
		ThrowIfDisposed();
		NativeLibrary.OPN_SetChannelMask(_handle, mask);
	}

	/// <summary>FM 音量を設定します (単位: dB)</summary>
	public void SetVolumeFM(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPN_SetVolumeFM(_handle, db);
	}

	/// <summary>PSG 音量を設定します (単位: dB)</summary>
	public void SetVolumePSG(int db)
	{
		ThrowIfDisposed();
		NativeLibrary.OPN_SetVolumePSG(_handle, db);
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
				NativeLibrary.OPN_Mix(_handle, p, nsamples);
			}
		}
	}

	/// <summary>タイマーを進め、マイクロ秒単位でカウントします</summary>
	public int Count(int us)
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_Count(_handle, us);
	}

	/// <summary>次のタイマーイベントまでのサンプル数を取得します</summary>
	public int GetNextEvent()
	{
		ThrowIfDisposed();
		return NativeLibrary.OPN_GetNextEvent(_handle);
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
