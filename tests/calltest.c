/*
  shared-lib-call-test

  Smoke test for fmgen C bridge API when built as a shared library.
  - Calls every function declared in bridge/fmgen_c.h at least once.
  - Does NOT validate audio correctness or write WAV.
  - OPNA rhythm sample loading failure is tolerated (external assets optional).
*/

#include <stdio.h>
#include <stdint.h>
#include <string.h>

#include "bridge/fmgen_c.h"

static int fail(const char* msg)
{
    fprintf(stderr, "FAIL: %s\n", msg);
    return 1;
}

int main(void)
{
    int32_t buf[64];
    memset(buf, 0, sizeof(buf));

    printf("PSG tests\n");
    PSGHandle* psg = PSG_Create();
    printf(" PSG_Create -> %p\n", (void*)psg);
    if (!psg) return fail("PSG_Create returned NULL");
    PSG_SetClock(psg, 3579545, 44100);
    PSG_Reset(psg);
    PSG_SetReg(psg, 0, 0);
    printf(" PSG_GetReg(0) -> %u\n", PSG_GetReg(psg, 0));
    PSG_SetVolume(psg, 0);
    PSG_SetChannelMask(psg, 0);
    PSG_Mix(psg, buf, 1);
    printf(" PSG_Mix -> %d,%d\n", (int)buf[0], (int)buf[1]);
    PSG_Destroy(psg);

    printf("OPN tests\n");
    OPNHandle* opn = OPN_Create();
    printf(" OPN_Create -> %p\n", (void*)opn);
    if (!opn) return fail("OPN_Create returned NULL");
    if (!OPN_Init(opn, 3579545, 44100, 0, NULL)) return fail("OPN_Init failed");
    if (!OPN_SetRate(opn, 3579545, 44100, 0)) return fail("OPN_SetRate failed");
    OPN_Reset(opn);
    OPN_SetReg(opn, 0, 0);
    printf(" OPN_GetReg(0) -> %u\n", OPN_GetReg(opn, 0));
    printf(" OPN_ReadStatus -> %u\n", OPN_ReadStatus(opn));
    OPN_SetChannelMask(opn, 0);
    OPN_SetVolumeFM(opn, 0);
    OPN_SetVolumePSG(opn, 0);
    OPN_Mix(opn, buf, 1);
    printf(" OPN_Mix -> %d,%d\n", (int)buf[0], (int)buf[1]);
    printf(" OPN_Count(1000) -> %d\n", OPN_Count(opn, 1000));
    printf(" OPN_GetNextEvent -> %d\n", (int)OPN_GetNextEvent(opn));
    OPN_Destroy(opn);

    printf("OPNA tests\n");
    OPNAHandle* opna = OPNA_Create();
    printf(" OPNA_Create -> %p\n", (void*)opna);
    if (!opna) return fail("OPNA_Create returned NULL");
    if (!OPNA_Init(opna, 8000000, 44100, 0, "./")) return fail("OPNA_Init failed");
    {
        int ok = OPNA_LoadRhythmSample(opna, "./");
        printf(" OPNA_LoadRhythmSample(\"./\") -> %d\n", ok);
        if (!ok) {
            printf("  (info) Rhythm samples not found; continuing.\n");
        }
    }
    if (!OPNA_SetRate(opna, 8000000, 44100, 0)) return fail("OPNA_SetRate failed");
    OPNA_Reset(opna);
    OPNA_SetReg(opna, 0, 0);
    printf(" OPNA_GetReg(0) -> %u\n", OPNA_GetReg(opna, 0));
    printf(" OPNA_ReadStatus -> %u\n", OPNA_ReadStatus(opna));
    printf(" OPNA_ReadStatusEx -> %u\n", OPNA_ReadStatusEx(opna));
    OPNA_SetChannelMask(opna, 0);
    OPNA_SetVolumeFM(opna, 0);
    OPNA_SetVolumePSG(opna, 0);
    OPNA_SetVolumeADPCM(opna, 0);
    OPNA_SetVolumeRhythmTotal(opna, 0);
    OPNA_SetVolumeRhythm(opna, 0, 0);
    printf(" OPNA_GetADPCMBuffer -> %p\n", (void*)OPNA_GetADPCMBuffer(opna));
    OPNA_Mix(opna, buf, 1);
    printf(" OPNA_Mix -> %d,%d\n", (int)buf[0], (int)buf[1]);
    printf(" OPNA_Count(1000) -> %d\n", OPNA_Count(opna, 1000));
    printf(" OPNA_GetNextEvent -> %d\n", (int)OPNA_GetNextEvent(opna));
    OPNA_Destroy(opna);

    printf("OPNB tests\n");
    OPNBHandle* opnb = OPNB_Create();
    printf(" OPNB_Create -> %p\n", (void*)opnb);
    if (!opnb) return fail("OPNB_Create returned NULL");
    {
        static uint8_t dummy_adpcma[1] = { 0 };
        static uint8_t dummy_adpcmb[256] = { 0 };
        if (!OPNB_Init(opnb, 8000000, 44100, 0,
                      dummy_adpcma, (int32_t)sizeof(dummy_adpcma),
                      dummy_adpcmb, (int32_t)sizeof(dummy_adpcmb))) {
            return fail("OPNB_Init failed");
        }
    }
    if (!OPNB_SetRate(opnb, 8000000, 44100, 0)) return fail("OPNB_SetRate failed");
    OPNB_Reset(opnb);
    OPNB_SetReg(opnb, 0, 0);
    printf(" OPNB_GetReg(0) -> %u\n", OPNB_GetReg(opnb, 0));
    printf(" OPNB_ReadStatus -> %u\n", OPNB_ReadStatus(opnb));
    printf(" OPNB_ReadStatusEx -> %u\n", OPNB_ReadStatusEx(opnb));
    OPNB_SetChannelMask(opnb, 0);
    OPNB_SetVolumeFM(opnb, 0);
    OPNB_SetVolumePSG(opnb, 0);
    OPNB_SetVolumeADPCMATotal(opnb, 0);
    OPNB_SetVolumeADPCMA(opnb, 0, 0);
    OPNB_SetVolumeADPCMB(opnb, 0);
    OPNB_Mix(opnb, buf, 1);
    printf(" OPNB_Mix -> %d,%d\n", (int)buf[0], (int)buf[1]);
    printf(" OPNB_Count(1000) -> %d\n", OPNB_Count(opnb, 1000));
    printf(" OPNB_GetNextEvent -> %d\n", (int)OPNB_GetNextEvent(opnb));
    OPNB_Destroy(opnb);

    printf("OPM tests\n");
    OPMHandle* opm = OPM_Create();
    printf(" OPM_Create -> %p\n", (void*)opm);
    if (!opm) return fail("OPM_Create returned NULL");
    if (!OPM_Init(opm, 3579545, 44100, 0)) return fail("OPM_Init failed");
    if (!OPM_SetRate(opm, 3579545, 44100, 0)) return fail("OPM_SetRate failed");
    OPM_Reset(opm);
    OPM_SetReg(opm, 0, 0);
    printf(" OPM_ReadStatus -> %u\n", OPM_ReadStatus(opm));
    OPM_SetChannelMask(opm, 0);
    OPM_SetVolume(opm, 0);
    OPM_Mix(opm, buf, 1);
    printf(" OPM_Mix -> %d,%d\n", (int)buf[0], (int)buf[1]);
    printf(" OPM_Count(1000) -> %d\n", OPM_Count(opm, 1000));
    printf(" OPM_GetNextEvent -> %d\n", (int)OPM_GetNextEvent(opm));
    OPM_Destroy(opm);

    printf("Done\n");
    return 0;
}
