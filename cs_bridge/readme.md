# FM Sound Generator with OPN/OPM interface

```
C++ による FM/PSG 音源の実装です。
AY8910, YM2203, YM2151, YM2608, YM2610 相当のインターフェースも実装してあります。

Copyright (C) by cisc 1998, 2003.

(readme-fmgem.txtより抜粋)
```

## fmgen.cs_bridge
- fmgenのC#用ブリッジインターフェイス
- windows(arm64/x64), macos(arm64/x64), linux(arm/arm64/x64)用のネイティブライブラリを同梱
- Nugetパッケージ

### バージョン履歴
- 0.8.19: 初期リリース、細かい調整やビルドパイプライン調整
- 0.8.20: upstreamのfmgen更新を反映
- 0.8.21: upstreamのfmgen更新を反映
- 0.8.22: upstreamのfmgen更新を反映

### ライセンスについて
- fmgenに関してはreadme-fmgen.txtに記載のライセンスに準じます
- フォーク元のコードに関してはそのライセンスに準じます
- このフォークオリジナルの部分に関しては[WTFPL](https://www.wtfpl.net/)とします
