namespace fmgen.cs_bridge;

/// <summary>
/// PSG (AY-3-8910 互換) 音源チップのラッパークラス
/// </summary>
/// <remarks>
/// Mix メソッドは「加算合成」を行います。呼び出し前にバッファをゼロクリアしてください
/// </remarks>
public sealed class PSG : IDisposable
{
	readonly NativeLibrary.PSGSafeHandle _handle;
	bool _disposed;

	/// <summary>PSG インスタンスを生成します</summary>
	public PSG()
	{
		var ptr = NativeLibrary.PSG_Create();
		if (ptr == 0) throw new InvalidOperationException("PSG_Create failed.");
		_handle = new NativeLibrary.PSGSafeHandle(ptr, ownsHandle: true);
	}

	/// <summary>PSG をリセットします</summary>
	public void Reset()
	{
		ThrowIfDisposed();
		NativeLibrary.PSG_Reset(_handle);
	}

	/// <summary>クロックとサンプリングレートを設定します</summary>
	public void SetClock(int clock, int rate)
	{
		ThrowIfDisposed();
		NativeLibrary.PSG_SetClock(_handle, clock, rate);
	}

	/// <summary>レジスタに値を書き込みます</summary>
	public void SetReg(uint regnum, byte data)
	{
		ThrowIfDisposed();
		NativeLibrary.PSG_SetReg(_handle, regnum, data);
	}

	/// <summary>レジスタの値を取得します</summary>
	public uint GetReg(uint regnum)
	{
		ThrowIfDisposed();
		return NativeLibrary.PSG_GetReg(_handle, regnum);
	}

	/// <summary>音量を設定します (単位: 約 1/2 dB)</summary>
	public void SetVolume(int vol)
	{
		ThrowIfDisposed();
		NativeLibrary.PSG_SetVolume(_handle, vol);
	}

	/// <summary>チャンネルマスクを設定します</summary>
	public void SetChannelMask(int mask)
	{
		ThrowIfDisposed();
		NativeLibrary.PSG_SetChannelMask(_handle, mask);
	}

	/// <summary>
	/// PCM を nsamples フレーム分合成し、dest に加算します（ステレオ interleaved: L,R,L,R,...）
	/// </summary>
	/// <param name="dest">ステレオ interleaved の int32 バッファ。長さは nsamples * 2 以上必要</param>
	/// <param name="nsamples">フレーム数（ステレオペア数）</param>
	public void Mix(Span<int> dest, int nsamples)
	{
		ThrowIfDisposed();
		if (dest.Length < nsamples * 2)
			throw new ArgumentException("Buffer too small.", nameof(dest));

		unsafe
		{
			fixed (int* p = dest)
			{
				NativeLibrary.PSG_Mix(_handle, p, nsamples);
			}
		}
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
