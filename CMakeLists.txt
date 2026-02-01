cmake_minimum_required(VERSION 3.13)

project(fmgen LANGUAGES C CXX)

# --- .NET RID 判定 ---
# - 手動での上書きは `-DDOTNET_RID=...` で可能
if(NOT DEFINED DOTNET_RID)
  # OS 部分を判定
  if(WIN32)
    set(RID_OS "win")
  elseif(APPLE)
    set(RID_OS "osx")
  else()
    set(RID_OS "linux")
  endif()

  # アーキテクチャ部分を判定
  if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    if(CMAKE_SYSTEM_PROCESSOR MATCHES "arm64|aarch64|ARM64")
      set(RID_ARCH "arm64")
    else()
      set(RID_ARCH "x64")
    endif()
  else()
    if(CMAKE_SYSTEM_PROCESSOR MATCHES "arm|ARM")
      set(RID_ARCH "arm")
    else()
      set(RID_ARCH "x86")
    endif()
  endif()

  set(DOTNET_RID "${RID_OS}-${RID_ARCH}")
endif()

message(STATUS "Using .NET RID: ${DOTNET_RID}")
# ネイティブ出力先ディレクトリ
set(NATIVE_OUTPUT_DIR "${CMAKE_SOURCE_DIR}/runtimes/${DOTNET_RID}/native")

add_library(fmgen SHARED
  fmgen/fmtimer.cpp
  fmgen/opna.cpp
  fmgen/fmgen.cpp
  fmgen/opm.cpp
  fmgen/psg.cpp
  bridge/fmgen_c.cpp
)

target_include_directories(fmgen PUBLIC .)
target_compile_definitions(fmgen PRIVATE FMGEN_C_EXPORTS)
set_target_properties(fmgen PROPERTIES CXX_VISIBILITY_PRESET hidden VISIBILITY_INLINES_HIDDEN YES)

# --- 出力ディレクトリ設定 ---
set_target_properties(fmgen PROPERTIES
  RUNTIME_OUTPUT_DIRECTORY "${NATIVE_OUTPUT_DIR}"
  LIBRARY_OUTPUT_DIRECTORY "${NATIVE_OUTPUT_DIR}"
)
# マルチ構成ジェネレータ（Visual Studio, Xcode 等）での Debug/Release サブディレクトリ生成を抑止
foreach(config IN ITEMS Debug Release RelWithDebInfo MinSizeRel)
  string(TOUPPER ${config} CONFIG_UPPER)
  set_target_properties(fmgen PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY_${CONFIG_UPPER} "${NATIVE_OUTPUT_DIR}"
    LIBRARY_OUTPUT_DIRECTORY_${CONFIG_UPPER} "${NATIVE_OUTPUT_DIR}"
  )
endforeach()

if (MSVC)
  target_compile_definitions(fmgen PRIVATE _CRT_SECURE_NO_WARNINGS)
  target_compile_options(fmgen PRIVATE /utf-8)
endif()

# 共有ライブラリの C API リンケージ確認用テスト実行ファイル
add_executable(call_test tests/calltest.c)
set_target_properties(call_test PROPERTIES OUTPUT_NAME "calltest")
target_link_libraries(call_test PRIVATE fmgen)

# ビルド後処理: テスト実行ファイルのあるディレクトリにネイティブライブラリをコピー
# これにより、`calltest.exe` 実行時に同ディレクトリ上の DLL/so を参照できるようにする
add_custom_command(TARGET call_test POST_BUILD
  COMMAND ${CMAKE_COMMAND} -E copy_if_different
    $<TARGET_FILE:fmgen>
    $<TARGET_FILE_DIR:call_test>
)
