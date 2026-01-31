# FM Sound Generator with OPN/OPM interface

```
C++ による FM/PSG 音源の実装です。
AY8910, YM2203, YM2151, YM2608, YM2610 相当のインターフェースも実装してあります。

Copyright (C) by cisc 1998, 2003.

(fmgen readme.txtより抜粋)
```

## このフォークについて

- https://github.com/minagisyu/fmgen ベース
- ネイティブライブラリビルド時に .NET RID に基づいたディレクトリに出力
- C# ブリッジインターフェイスを追加
- ネイティブライブラリ同梱のNuGetパッケージを出力
- ブリッジインターフェイス呼び出しテストスクリプトを追加
- GitHub Actions による CI/CD を追加

### ビルドについて
```
mkdir build
cd build 
cmake ../
cmake --build . --config Release
```

### ライセンスについて
- fmgen に関しては readme-fmgen.txt に記載のライセンスに準じます
- このフォークオリジナルの部分に関しては[WTFPL](https://www.wtfpl.net/)とします
