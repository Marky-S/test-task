using System;
using System.Collections.Generic;
using RAGE;
using static RAGE.Game.Misc;

namespace car_dirty
{
    public class Main : Events.Script
    {
        public Main()
        {
            Events.OnPlayerChat += OnClientChat;
            Events.Tick += CheckControls;
        }

        private void CheckControls(List<Events.TickNametagData> _)
        {
            if(RAGE.Game.Pad.IsControlJustPressed(0, 174))
            {
                StartCutscenesCheck(CurrentCutsceneIdx - 1);
            }
            else if (RAGE.Game.Pad.IsControlJustPressed(0, 175))
            {
                StartCutscenesCheck(CurrentCutsceneIdx + 1);
            }
        }

        private async void OnClientChat(string text, Events.CancelEventArgs cancel)
        {
            if(text.Contains("cutscenes"))
            {
                string number= text.Substring(9);

                try
                {
                    StartCutscenesCheck(Convert.ToInt32(number));
                }
                catch 
                {
                    StartCutscenesCheck();
                }
            }
            if (text.Contains("coords"))
            {
                RAGE.Chat.Output(RAGE.Elements.Player.LocalPlayer.Vehicle.Position.ToString());
                RAGE.Chat.Output(RAGE.Elements.Player.LocalPlayer.Vehicle.GetRotation(2).ToString());
            }
            else if(text.Contains("script"))
            {
                string scriptName = text.Substring(7);

                if(!RAGE.Game.Script.DoesScriptExist(scriptName))
                {
                    RAGE.Chat.Output($"Script [{scriptName}] unknown.");
                    return;
                }


                RAGE.Game.Script.RequestScript(scriptName);

                int time = 0;

                while (!RAGE.Game.Script.HasScriptLoaded(scriptName))
                {
                    time++;
                    if (time > 500)
                    {
                        Chat.Output($"Skiped");
                        return;
                    }

                    await Task.WaitAsync(1);
                }

                RAGE.Chat.Output($"Script [{scriptName}] loaded.");

                RAGE.Game.Utils.StartNewScript(scriptName, 1000);
            }
            else if (text.Contains("recording"))
            {
                string recordingName = "carwash2_r";

                RAGE.Game.Ai.RequestWaypointRecording(recordingName);

                while(!RAGE.Game.Ai.GetIsWaypointRecordingLoaded(recordingName))
                {
                    await RAGE.Task.WaitAsync(1);
                }

                RAGE.Chat.Output($"Recording loaded.");

                /*int sequence = 0;
                int pedHandle = RAGE.Elements.Player.LocalPlayer.Handle;

                int vehHandle = RAGE.Elements.Player.LocalPlayer.Vehicle.Handle;

                RAGE.Game.Ai.TaskStandStill(pedHandle, 3000);
                RAGE.Game.Ai.CloseSequenceTask(sequence);
                RAGE.Game.Ai.TaskPerformSequence(pedHandle, sequence);
                RAGE.Game.Ai.ClearSequenceTask(ref sequence);
                RAGE.Game.Ai.VehicleWaypointPlaybackOverrideSpeed(vehHandle, 1.5f);

                RAGE.Game.Ai.OpenSequenceTask(ref sequence);
                RAGE.Game.Ai.TaskVehicleFollowWaypointRecording
                (
                    pedHandle,
                    vehHandle,
                    recordingName,
                    262144,
                    0,
                    546,
                    -1,
                    1.5f,
                    false,
                    1.25f
                );
                */

                var rollerPos = RAGE.Elements.Player.LocalPlayer.Position + new Vector3(0, 0, 1f);

                int brush = RAGE.Game.Object.CreateObject(GetHashKey("prop_carwash_roller_horz"), rollerPos.X, rollerPos.Y, rollerPos.Z, false, true, false);
                while(!RAGE.Game.Entity.DoesEntityExist(brush))
                {
                    await Task.WaitAsync(1);
                }

                RAGE.Game.Audio.StartAudioScene("CAR_WASH_SCENE");
                RAGE.Game.Audio.RequestScriptAudioBank("SCRIPT/CARWASH", false, -1);
                RAGE.Game.Audio.PlaySoundFromEntity(-1, "BRUSHES_SPINNING", brush, "CARWASH_SOUNDS", false, 0);
                RAGE.Game.Graphics.StartParticleFxLoopedOnEntity("ent_amb_car_wash", brush, 0, 0, 0, 0, 90, 0, 1f, false, false, false);
            }
            else if (text.Contains("wash"))
            {
                string scriptName = "carwash2";
                
                RAGE.Game.Script.RequestScript(scriptName);

                Chat.Output("Wash requested");

                while (!RAGE.Game.Script.HasScriptLoaded(scriptName))
                {
                    await Task.WaitAsync(1);
                }

                Chat.Output("Wash started");

                RAGE.Game.Utils.StartNewScript(scriptName, 1424);

                RAGE.Game.Script.SetScriptAsNoLongerNeeded(scriptName);
                
            }
            else if (text.Contains("tp"))
            {
                text = text.Substring(3);

                string[] coords = text.Split(',');

                Chat.Output($"{text} {RAGE.Util.MsgPack.ConvertToJson(RAGE.Util.MsgPack.Serialize(coords))}");

                RAGE.Elements.Player.LocalPlayer.Position = new Vector3(Convert.ToSingle(coords[0].ToString().Replace('.', ',')), Convert.ToSingle(coords[1].ToString().Replace('.', ',')), Convert.ToSingle(coords[2].ToString().Replace('.', ',')));
            }
        }

        public List<string> AllCutscenes = new List<string>
        {
            "abigail_mcs_1_concat",
            "abigail_mcs_2",
            "ac_ig_3_p3_b(1)",
            "ac_ig_3_p3_b(2)",
            "ac_ig_3_p3_b",
            "ah_1_ext_t6(1)",
            "ah_1_ext_t6",
            "ah_1_int",
            "ah_1_mcs_1(1)",
            "ah_1_mcs_1",
            "ah_2_ext_alt",
            "ah_2_ext_p4",
            "ah_3a_explosion",
            "ah_3a_ext",
            "ah_3a_int",
            "ah_3a_mcs_3",
            "ah_3a_mcs_6",
            "ah_3b_ext",
            "ah_3b_int",
            "ah_3b_mcs_1",
            "ah_3b_mcs_2",
            "ah_3b_mcs_3",
            "ah_3b_mcs_4",
            "ah_3b_mcs_5",
            "ah_3b_mcs_6_p1",
            "ah_3b_mcs_7",
            "apa_fin_cel_apt2",
            "apa_fin_cel_apt3",
            "apa_fin_cel_apt4",
            "apa_hum_fin_int",
            "apa_nar_int",
            "apa_nar_mid",
            "apa_pri_int",
            "arena_int",
            "arena_int2",
            "armenian_1_int",
            "armenian_1_mcs_1",
            "armenian_2_int",
            "armenian_2_mcs_6",
            "armenian_3_int",
            "armenian_3_mcs_3",
            "armenian_3_mcs_4",
            "armenian_3_mcs_5",
            "armenian_3_mcs_6",
            "armenian_3_mcs_7",
            "armenian_3_mcs_8",
            "armenian_3_mcs_9_concat",
            "arm_1_mcs_2_concat",
            "arm_2_mcs_4",
            "ass_int_2_alt1",
            "ass_mcs_1",
            "bar_1_rcm_p2(1)",
            "bar_1_rcm_p2",
            "bar_2_rcm",
            "bar_3_rcm",
            "bar_4_rcm",
            "bar_5_rcm_p2(1)",
            "bar_5_rcm_p2",
            "bmad_intro",
            "bss_1_mcs_2",
            "bss_1_mcs_3",
            "bs_1_int",
            "bs_2a_2b_int",
            "bs_2a_ext",
            "bs_2a_int",
            "bs_2a_mcs_1",
            "bs_2a_mcs_10",
            "bs_2a_mcs_11",
            "bs_2a_mcs_2",
            "bs_2a_mcs_3_alt",
            "bs_2a_mcs_4",
            "bs_2a_mcs_5",
            "bs_2a_mcs_6",
            "bs_2a_mcs_7_p1",
            "bs_2a_mcs_8",
            "bs_2a_mcs_8_p3",
            "bs_2b_ext_alt1a",
            "bs_2b_ext_alt2",
            "bs_2b_int(1)",
            "bs_2b_int",
            "bs_2b_mcs_1",
            "bs_2b_mcs_3",
            "bunk_int",
            "carbine_mag_offset_test",
            "car_1_ext_concat",
            "car_1_int_concat",
            "car_2_mcs_1(1)",
            "car_2_mcs_1",
            "car_2_mcs_1_merged",
            "car_4_ext",
            "car_4_mcs_1",
            "car_5_ext",
            "car_5_mcs_1",
            "car_steal_3_mcs_1",
            "car_steal_3_mcs_2",
            "car_steal_3_mcs_3",
            "chinese_1_int",
            "chinese_2_int",
            "chi_1_mcs_1",
            "chi_1_mcs_4_concat",
            "chi_2_mcs_5",
            "choice_int",
            "cletus_mcs_1_concat",
            "club_intro",
            "club_intro2",
            "club_open",
            "dhp1_mcs_1",
            "dixn_intro",
            "drf_mic_1_cs_1",
            "drf_mic_1_cs_2",
            "drf_mic_2_cs_1",
            "drf_mic_2_cs_2",
            "drf_mic_3_cs_1(1)",
            "drf_mic_3_cs_1",
            "drf_mic_3_cs_2",
            "ef_1_rcm",
            "ef_2_rcm",
            "ef_3_rcm_concat",
            "eps_4_mcs_1",
            "eps_4_mcs_2",
            "eps_4_mcs_3",
            "ep_1_rcm_concat",
            "ep_2_rcm",
            "ep_3_rcm_alt1",
            "ep_4_rcm",
            "ep_4_rcm_concat(1)",
            "ep_4_rcm_concat",
            "ep_5_rcm",
            "vep_6_rcm",
            "ep_7_rcm",
            "ep_8_rcm(1)",
            "ep_8_rcm",
            "es_1_rcm_concat",
            "es_1_rcm_p1",
            "es_2_rcm_concat",
            "es_3_mcs_1",
            "es_3_mcs_2",
            "es_3_rcm",
            "es_4_rcm_p1",
            "es_4_rcm_p2_concat",
            "exile_1_int",
            "exile_2_int",
            "exile_3_int",
            "exl_1_mcs_1_p3_b",
            "exl_2_mcs_3",
            "exl_3_ext",
            "family_1_int",
            "family_2_int",
            "family_2_mcs_2",
            "family_2_mcs_3",
            "family_2_mcs_4",
            "family_3_ext(1)",
            "family_3_ext",
            "family_3_int(1)",
            "family_3_int",
            "family_3_intp1",
            "family_3_intp1_2",
            "family_3_intp1_3",
            "family_3_intp2",
            "family_3_intp2_1",
            "family_3_mcs_1",
            "family_4_mcs_2",
            "family_4_mcs_3_concat",
            "family_5_int",
            "family_5_mcs_1",
            "family_5_mcs_2(1)",
            "family_5_mcs_2",
            "family_5_mcs_3",
            "family_5_mcs_4",
            "family_5_mcs_5",
            "family_5_mcs_5_p4(1)",
            "family_5_mcs_5_p4",
            "family_5_mcs_5_p5",
            "fam_1_ext_2",
            "fam_1_ext_alt2",
            "fam_1_ext_alt3",
            "fam_1_mcs_2",
            "fam_3_mcs_1(1)",
            "fam_3_mcs_1",
            "fam_3_mcs_1_b",
            "fam_4_int_alt1",
            "fam_5_mcs_6(1)",
            "fam_5_mcs_6",
            "fam_5_mcs_6_p3_b",
            "fam_5_mcs_6_p4",
            "fam_6_int",
            "fam_6_int_p3_t7",
            "fam_6_mcs_1",
            "fam_6_mcs_2_concat",
            "fam_6_mcs_3",
            "fam_6_mcs_4",
            "fam_6_mcs_5",
            "fam_6_mcs_6",
            "fam_6_mcs_6_p4_b",
            "fbi_1_ext",
            "fbi_1_int",
            "fbi_1_jan_kill",
            "fbi_1_mcs_1_concat",
            "fbi_2_ext",
            "fbi_2_int",
            "fbi_2_mcs_1",
            "fbi_2_mcs_2",
            "fbi_2_mcs_3b",
            "fbi_2_mcs_3_p1a",
            "fbi_3_int",
            "fbi_3_mcs_1",
            "fbi_3_mcs_2",
            "fbi_3_mcs_3",
            "fbi_3_mcs_4p2",
            "fbi_3_mcs_5",
            "fbi_3_mcs_5p2",
            "fbi_3_mcs_6p1_b",
            "fbi_3_mcs_6p2",
            "fbi_3_mcs_7",
            "fbi_3_mcs_8",
            "fbi_4_int",
            "fbi_4_mcs_2_concat",
            "fbi_4_mcs_3_concat",
            "fbi_5a_mcs_1",
            "fbi_5a_mcs_10",
            "fbi_5b_mcs_1",
            "fbi_5_ext",
            "fbi_5_int",
            "fin_a_ext",
            "fin_a_int",
            "fin_a_mcs_1",
            "fin_b_ext",
            "fin_b_mcs_1_aandb",
            "fin_b_mcs_2",
            "fin_c2_mcs_1",
            "fin_c2_mcs_5",
            "fin_c_ext",
            "fin_c_int",
            "fin_c_mcs_1",
            "fin_c_mcs_1_p1_a",
            "fin_ext_p1",
            "fin_ext_p2",
            "fos_ep_1_p0",
            "fos_ep_1_p1",
            "fos_ep_1_p2",
            "fos_ep_1_p3",
            "fos_ep_1_p3b",
            "fos_ep_1_p3c",
            "fos_ep_1_p3d",
            "fos_ep_1_p3e",
            "fos_ep_1_p4",
            "fos_ep_1_p5",
            "fos_ep_1_p5b",
            "fos_ep_1_p6",
            "fos_ep_1_p6b",
            "fos_ep_1_p7",
            "fos_ep_1_p8",
            "fos_ep_1_p9",
            "fos_ep_2_p1",
            "fos_ep_2_p2",
            "fos_ep_2_p3",
            "fos_ep_2_p5b",
            "fos_ep_2_p5c",
            "fos_ep_2_p5d",
            "fos_ep_2_p6_alt1",
            "fos_tracey_1_p1",
            "fos_tracey_1_p2",
            "fos_tracey_1_p2b",
            "fos_tracey_1_p3",
            "franklin_1_int(1)",
            "franklin_1_int",
            "franklin_1_intp1",
            "franklin_1_intp2",
            "franklin_1_intp3",
            "fra_0_int",
            "fra_0_int_p1_alt",
            "fra_0_mcs_1",
            "fra_0_mcs_4_p2_t3",
            "fra_0_mcs_5_p1",
            "fra_1_mcs_1",
            "fra_2_ext",
            "fra_2_ig_4_alt1_concat",
            "fra_2_int",
            "handposes",
            "hang_int",
            "hang_int_plane",
            "hao_mcs_1",
            "heist_int",
            "hs3f_all_drp1",
            "hs3f_all_drp2",
            "hs3f_all_drp3",
            "hs3f_all_esc",
            "hs3f_all_esc_b",
            "hs3f_dir_ent",
            "hs3f_dir_sew",
            "hs3f_int1",
            "hs3f_int2",
            "hs3f_mul_rp1",
            "hs3f_mul_rp2",
            "hs3f_mul_vlt",
            "hs3f_sub_cel",
            "hs3f_sub_vlt",
            "hs3_arc_int",
            "hs3_cel_arc",
            "hs3_ext",
            "hs3_int",
            "hs3_pln_int",
            "hs3_prp_celb",
            "hs3_prp_vin1",
            "hs3_prp_vin2",
            "hun_2_mcs_1",
            "iaaj_ext",
            "iaaj_int",
            "iaa_int",
            "impexp_int",
            "impexp_int_l1",
            "jh2_fina_mcs4_a1a2",
            "jhrs_1_int_p2",
            "jhrs_1_int_p3",
            "jhrs_1_int_p4",
            "jhrs_2_hero_shot",
            "jhrs_2_hero_shot_a",
            "jhrs_2_int_p1_t2",
            "jhrs_2_int_p1_t3",
            "jhrs_2_int_p2",
            "jhrs_2_int_p2_t5",
            "jhrs_2_int_p3a",
            "jhrs_2_int_p3_t2",
            "jhrs_2_int_p3_t3",
            "jhrs_2_int_p4",
            "jhrs_2_int_p5",
            "jhrs_2_int_p6",
            "jhrs_3_int_p2a",
            "jhrs_4_int_p1",
            "jhrs_5_int_p1",
            "jhrs_5_int_p1b",
            "jhrs_5_int_p2",
            "jhrs_5_int_p3_t1",
            "jhrs_5_int_p3_t3",
            "jhrs_6_int_p1",
            "jhrs_6_int_p2",
            "jhrs_6_int_p3",
            "jhrs_6_int_p4",
            "jh_1_ig_3",
            "jh_1_int",
            "jh_1_mcs_4p2",
            "jh_1_mcs_4_p1_alt1",
            "jh_2a_intp4",
            "jh_2a_mcs_1",
            "jh_2b_int",
            "jh_2b_mcs_1",
            "jh_2_arrest_fail_c",
            "jh_2_celeb",
            "jh_2_fin_a_mcs4_a1",
            "jh_end_pt2_a1_p2_a",
            "josh_1_int",
            "josh_1_int_concat(1)",
            "josh_1_int_concat(2)",
            "josh_1_int_concat",
            "josh_2_intp1_t4(1)",
            "josh_2_intp1_t4",
            "josh_3_intp1(1)",
            "josh_3_intp1",
            "josh_4_int_concat",
            "lamar_1_int",
            "lam_1_mcs_1_concat(1)",
            "lam_1_mcs_1_concat",
            "lam_1_mcs_2",
            "lam_1_mcs_3",
            "lester_1_int",
            "les_1a_mcs_0",
            "les_1a_mcs_1",
            "les_1a_mcs_2",
            "les_1a_mcs_3",
            "les_1a_mcs_4",
            "les_1b_mcs_1",
            "low_drv_ext",
            "low_drv_int",
            "low_fin_ext",
            "low_fin_int",
            "low_fin_mcs1",
            "low_fun_ext",
            "low_fun_int",
            "low_fun_mcs1",
            "low_int",
            "low_pho_ext",
            "low_pho_int",
            "low_tra_ext",
            "low_tra_int",
            "lsdhs_mcs_2",
            "lsdhs_mcs_3_p1_concat",
            "lsdhs_mcs_3_p2",
            "lsdh_2a_ext",
            "lsdh_2a_int",
            "lsdh_2a_rf_01",
            "lsdh_2b_int",
            "lsdh_2b_mcs_1",
            "lsdh_int",
            "martin_1_ext",
            "martin_1_int(1)",
            "martin_1_int",
            "martin_1_mcs_1",
            "maude_mcs_1(1)",
            "maude_mcs_1",
            "maude_mcs_2",
            "maude_mcs_3",
            "maude_mcs_4",
            "maude_mcs_5",
            "melt_int_10_p1",
            "melt_int_10_p2",
            "melt_int_1_p1",
            "melt_int_1_p1_t4",
            "melt_int_1_p2",
            "melt_int_2",
            "melt_int_3",
            "melt_int_4",
            "melt_int_5",
            "melt_int_6",
            "melt_int_7",
            "melt_int_8_p1",
            "melt_int_8_p2",
            "melt_int_8_p3",
            "melt_int_9",
            "merge_test_crowd1",
            "merge_test_doug",
            "mic_1_int",
            "mic_1_mcs_1",
            "mic_1_mcs_2",
            "mic_1_mcs_3",
            "mic_2_int",
            "mic_2_mcs_1",
            "mic_2_mcs_3_concat",
            "mic_3_ext(1)",
            "mic_3_ext",
            "mic_3_int(1)",
            "mic_3_int",
            "mic_3_int_p1_alt(1)",
            "mic_3_int_p1_alt",
            "mic_3_mcs_1_p1_a2",
            "mic_4_int",
            "mmb_1_rcm",
            "mmb_2_rcm",
            "mmb_3_rcm(1)",
            "mmb_3_rcm",
            "mpcas1_ext",
            "mpcas1_int",
            "mpcas2_ext",
            "mpcas2_int",
            "mpcas2_mcs1",
            "mpcas3_ext",
            "mpcas3_int",
            "mpcas3_mcs1",
            "mpcas4_int",
            "mpcas5_ext",
            "mpcas5_int",
            "mpcas6_ext",
            "mpcas6_int",
            "mpcas6_mcs1",
            "mpcas_int",
            "mph_fin_cel_apt",
            "mph_fin_cel_apt1",
            "mph_fin_cel_bar",
            "mph_fin_cel_roo",
            "mph_fin_cel_str",
            "mph_fin_cel_tre",
            "mph_hum_arm_ext",
            "mph_hum_del_ext",
            "mph_hum_emp_ext",
            "mph_hum_fin_ext",
            "mph_hum_fin_int",
            "mph_hum_fin_mcs1",
            "mph_hum_int",
            "mph_hum_key_ext",
            "mph_hum_key_mcs1",
            "mph_hum_mid",
            "mph_hum_val_ext",
            "mph_nar_bik_ext",
            "mph_nar_cok_ext",
            "mph_nar_fin_ext",
            "mph_nar_fin_int",
            "mph_nar_int",
            "mph_nar_met_ext",
            "mph_nar_mid",
            "mph_nar_tra_ext",
            "mph_nar_wee_ext",
            "mph_pac_bik_ext",
            "mph_pac_con_ext",
            "mph_pac_fin_ext",
            "mph_pac_fin_int",
            "mph_pac_fin_mcs0",
            "mph_pac_fin_mcs1",
            "mph_pac_fin_mcs2",
            "mph_pac_hac_ext",
            "mph_pac_hac_mcs1",
            "mph_pac_int",
            "mph_pac_mid",
            "mph_pac_pho_ext",
            "mph_pac_wit_mcs1",
            "mph_pac_wit_mcs2",
            "mph_pri_bus_ext",
            "mph_pri_fin_ext",
            "mph_pri_fin_int",
            "mph_pri_fin_mcs1",
            "mph_pri_fin_mcs2",
            "mph_pri_int",
            "mph_pri_mid",
            "mph_pri_pla_ext",
            "mph_pri_sta_ext",
            "mph_pri_sta_mcs1",
            "mph_pri_sta_mcs2",
            "mph_pri_unf_ext",
            "mph_pri_unf_mcs1",
            "mph_tut_car_ext",
            "mph_tut_ext",
            "mph_tut_fin_int",
            "mph_tut_int",
            "mph_tut_mcs1",
            "mph_tut_mid",
            "mpsui_int",
            "mp_intro_concat(1)",
            "mp_intro_concat",
            "mp_intro_mcs_10_a1",
            "mp_intro_mcs_10_a2",
            "mp_intro_mcs_10_a3",
            "mp_intro_mcs_10_a4",
            "mp_intro_mcs_10_a5",
            "mp_intro_mcs_11",
            "mp_intro_mcs_11_a1",
            "mp_intro_mcs_12_a1",
            "mp_intro_mcs_12_a2",
            "mp_intro_mcs_12_a3",
            "mp_intro_mcs_13",
            "mp_intro_mcs_14_b",
            "mp_intro_mcs_16_a1",
            "mp_intro_mcs_16_a2",
            "mp_intro_mcs_17_a5",
            "mp_intro_mcs_17_a8",
            "mp_intro_mcs_17_a9",
            "mp_intro_mcs_8_a1",
            "mp_intro_mcs_8_a1_cc",
            "mp_int_mcs_12_a3_3",
            "mp_int_mcs_12_a3_4",
            "mp_int_mcs_15_a1_b",
            "mp_int_mcs_15_a2b",
            "mp_int_mcs_15_a3",
            "mp_int_mcs_15_a4",
            "mp_int_mcs_17_a1",
            "mp_int_mcs_17_a2",
            "mp_int_mcs_17_a3",
            "mp_int_mcs_17_a4",
            "mp_int_mcs_17_a6",
            "mp_int_mcs_17_a7",
            "mp_int_mcs_18_a1",
            "mp_int_mcs_18_a2",
            "mp_int_mcs_5_alt1(1)",
            "mp_int_mcs_5_alt1",
            "mp_int_mcs_5_alt2(1)",
            "mp_int_mcs_5_alt2",
            "mp_int_mcs_7_a1",
            "nextgen_fx_test",
            "ng_optimise_concat",
            "ng_optimise_test",
            "nmt_1_rcm",
            "nmt_2_mcs_2",
            "nmt_2_rcm",
            "nmt_3_rcm",
            "oscar_mcs_1",
            "paper_1_rcm_alt1",
            "pap_1_mcs_1",
            "pap_1_rcm",
            "pap_2_mcs_1",
            "pap_2_rcm_p2",
            "pap_3_rcm",
            "pap_4_rcm",
            "pro_mcs_1",
            "pro_mcs_2",
            "pro_mcs_3_pt1",
            "pro_mcs_5",
            "pro_mcs_6(1)",
            "pro_mcs_6",
            "pro_mcs_6_p1",
            "pro_mcs_6_p2_a1_b",
            "pro_mcs_6_p2_alt1",
            "pro_mcs_7_concat",
            "rbhs_int",
            "rbhs_mcs_1",
            "rbhs_mcs_3",
            "rbhs_msc_3_p3",
            "rbh_2ab_mcs_6",
            "rbh_2a_ext1_a1_p1",
            "rbh_2a_ext_1",
            "rbh_2a_int",
            "rbh_2a_mcs_2_p3",
            "rbh_2a_mcs_2_p7",
            "rbh_2a_mcs_4",
            "rbh_2a_mcs_5",
            "sas_1_rcm_concat",
            "sas_2_rcm_t7",
            "scrap_1_rcm",
            "scrap_2_rcm",
            "sdrm_mcs_2",
            "silj_ext",
            "silj_int",
            "silj_mcs1",
            "silj_mcs2",
            "sil_int",
            "sil_pred_mcs1",
            "smun_intro",
            "smun_intro2",
            "sol_1_ext",
            "sol_1_int_alt",
            "sol_1_mcs_1_concat",
            "sol_1_mcs_2",
            "sol_1_mcs_3",
            "sol_2_ext_concat",
            "sol_2_int_alt1(1)",
            "sol_2_int_alt1",
            "sol_3_int",
            "sol_5_mcs_1",
            "sol_5_mcs_2",
            "sol_5_mcs_2_p5",
            "subj_ext",
            "subj_mcs0",
            "subj_mcs1",
            "sub_int",
            "tale_intro",
            "testbed_veh_blend",
            "test_arm_2_mcs_6",
            "test_jh_2a_intp4",
            "tmom_1_rcm",
            "tmom_2_rcm",
            "tonya_mcs_1",
            "tonya_mcs_2",
            "tonya_mcs_3",
            "trevor_1_int",
            "trevor_2_int",
            "trevor_drive_int",
            "trv2_mcs_8",
            "trvram_1",
            "trvram_2_concat",
            "trvram_3",
            "trvram_4",
            "trvram_5_con",
            "trv_1_mcs_1_p1",
            "trv_1_mcs_3_concat",
            "trv_1_mcs_4",
            "trv_2_mcs_4_concat",
            "trv_2_mcs_6",
            "trv_4_mcs_2",
            "trv_5_ext",
            "trv_5_int",
            "trv_dri_ext(1)",
            "trv_dri_ext",
            "trv_dri_mcs_concat",
            "under_int_1",
            "under_int_1_p2",
            "under_int_2",
            "under_int_2_p2",
            "under_int_3",
            "under_int_4",
            "under_int_4_p2",
            "under_int_4_p3",
            "under_int_5",
            "under_int_5_p2",
            "under_int_6",
            "under_int_6_p2",
            "under_int_6_p3",
            "under_int_7",
        };
        public string CurrentCutscene;
        public int CurrentCutsceneIdx;

        private async void StartCutscenesCheck(int number = 0)
        {
            Chat.Output($"Trying cutscene {AllCutscenes[number]}({number})");

            CurrentCutsceneIdx = number;
            CurrentCutscene = AllCutscenes[CurrentCutsceneIdx];

            int localPedID = RAGE.Elements.Player.LocalPlayer.Handle;

            RAGE.Game.Cutscene.RequestCutscene(CurrentCutscene, 8);

            RAGE.Game.Cutscene.RegisterEntityForCutscene(localPedID, "MP_1", 0, 0, 64);

            Chat.Output($"Loading");

            int time = 0;

            while (!RAGE.Game.Cutscene.HasCutsceneLoaded())
            { 
                time++;
                if (time > 500)
                {
                    Chat.Output($"Skiped");
                    RAGE.Game.Cutscene.RemoveCutscene();
                    return;
                }

                await Task.WaitAsync(1);
            }
            Chat.Output($"Loaded");

            RAGE.Game.Cutscene.StartCutscene(0);
        }
    }
}
