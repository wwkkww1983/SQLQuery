﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using PylonLiveView;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace PylonLiveView
{
    public class MyFunctions
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(
                            IntPtr hwnd,
                            int wMsg,
                            IntPtr wParam,
                            IntPtr lParam);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);

        /// <summary>
        /// 获得接收消息标志字符  E\H\R....
        /// </summary>
        /// <param name="str">需要带前缀?或!</param>
        /// <returns></returns>
        public string getTagString(string str)
        {
            return str.Substring(1, 1);
        }

        //获得校验码
        public string getCRCCode(string str)
        {
            try
            {
                return str.Substring(str.IndexOf('#') + 1, 4);
            }
            catch { return ""; }
        }

        //获得收到信息中的有效字串(除?/@和#+校验码外的字符)
        public string getValidStringMsg(string str)
        {
            try
            {
                str = str.Substring(1);
                str = str.Substring(0, str.IndexOf("#"));
                return str;
            }
            catch { return ""; }
        }

        //CRC8位校验
        public string CRC8(string str)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(str);
            short crc = 0;
            for (int j = 0; j < buffer.Length; j++)
            {
                crc ^= (Int16)(buffer[j] << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) > 0)
                    {
                        crc = (Int16)((crc << 1) ^ 0x1021);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            return string.Format(Convert.ToString(crc, 16).ToUpper().PadLeft(4, '0'), "0000");
        }

        public Bitmap copyImage(Bitmap sourceBmp, int startX, int startY, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            if ((startX + width >= sourceBmp.Width)
                || (startY + height >= sourceBmp.Height))
            {
                return bitmap;
            }
            try
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(sourceBmp, new Rectangle(0, 0, bitmap.Width, bitmap.Height)
                    , startX, startY, width, height, GraphicsUnit.Pixel);
                g.Save();
                return bitmap;
            }
            catch { return bitmap; }
            finally { }
        }

        public void SaveResultINIString(string Sheetbarcode, string str)
        {
            try
            {
                string resultDicPath = GlobalVar.gl_Directory_savePath; //Application.StartupPath + "\\" + GlobalVar.gl_folderName_ResultSave;
                if (!Directory.Exists(resultDicPath))
                { Directory.CreateDirectory(resultDicPath); }
                string resultPath = resultDicPath + "\\" + Sheetbarcode + ".ini";

                FileStream FS = new FileStream(resultPath, FileMode.Create);
                StreamWriter SW = new StreamWriter(FS);
                SW.WriteLine(str);
                SW.Close();
                SW.Dispose();
            }
            catch (Exception ex)
            {
                logWR.appendNewLogMessage("保存测试数据异常:"+ex.Message);
            }
        }
        string userName = "mmcs\\santec";
        string passWord = "Mektec01!";
        string netPath = @"\\192.168.208.237\share";
        //登录网络共享，每天要登录一次否则会断开
        public void LoadShare()
        {
            Process proc = new Process();
            try
            {
                string dosLine = @"net use " + netPath + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                //dosLine += "\r\n" + "del update.bat";
                //File.WriteAllText("update.bat", dosLine, ASCIIEncoding.Default);
                //System.Diagnostics.Process.Start("update.bat");

                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                //GlobalVar.gl_lastLoadNetTime = DateTime.Now;  //成功登录共享盘时间
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录共享盘异常：" + ex.ToString());
                appendNewLogMessage("登录共享盘异常：" + ex.ToString(), "NetError.text");
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
        }
        public void appendNewLogMessage(string str, string filename)
        {
            try
            {
                string dirName = Application.StartupPath + "\\LOG\\";
                if (!Directory.Exists(dirName))
                { Directory.CreateDirectory(dirName); }

                string _logfile = dirName + filename;
                FileStream FS = new FileStream(_logfile, FileMode.Append);
                string str_record = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\0\0\0\0" + str;
                StreamWriter SW = new StreamWriter(FS);
                SW.WriteLine(str_record);
                SW.Close();
                SW.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// 检验输入是否合法
        /// </summary>
        /// <param name="str">检测字串</param>
        /// <param name="checkType">1：数字  2：英文字符  3：数字+英文字符</param>
        /// <returns></returns>
        public bool checkStringIsLegal(string str, int checkType)
        {
            bool result = true;
            if (checkType == 1)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    result &= ((c >= 48) && (c <= 57));
                }
            }
            else if (checkType == 2)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    result &= (((c >= 65) && (c <= 90))
                        || ((c >= 97) && (c <= 122)));
                }
            }
            else if (checkType == 3)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    result &= ((((c >= 65) && (c <= 90)) || ((c >= 97) && (c <= 122)))
                        || ((c >= 48) && (c <= 57)));
                }
            }
            return result;
        }

        public void CheckFileExit()
        {
            try
            {
                string iniFilePath = GlobalVar.gl_strTargetPath + "\\" + GlobalVar.gl_iniTBS_FileName;
                if (!System.IO.File.Exists(iniFilePath))
                {
                    FileStream file = new FileStream(iniFilePath, FileMode.Create);
                    file.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// 获得本机IP
        /// </summary>
        /// <returns></returns>
        public IPAddress getHostIP()
        {
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork") //排除IPV6
                {
                    return _IPAddress;
                }
            }
            return IPAddress.Parse("127.0.0.1");
        }

        //判斷IP是否合格
        public bool checkIPStringIsLegal(string str)
        {
            try
            {
                Regex reg = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
                if (!reg.IsMatch(str))
                {
                    return false;
                }
                string[] array_str = str.Split('.');
                for (int i = 0; i < array_str.Length; i++)
                {
                    if (Convert.ToInt32(array_str[i]) > 255) { return false; }
                }
                return true;
            }
            catch
            { return false; }
        }

        //保存设置到TBS.INI
        public void WriteGlobalInfoToTBS()
        {
            try
            {
                #region 写入测试参数CONFIG.INI
                string iniFilePath = GlobalVar.gl_strTargetPath + "\\" + GlobalVar.gl_iniTBS_FileName;
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_MinMatchScore, GlobalVar.gl_MinMatchScore.ToString(), iniFilePath);      //匹配值   
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_TimeOut, GlobalVar.gl_decode_timeout.ToString(), iniFilePath);      //解析超時时长 
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_DecodeTimes, GlobalVar.gl_decode_times.ToString(), iniFilePath);      //解析次数              
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SheetCount, GlobalVar.gl_OneSheetCount.ToString(), iniFilePath);    //整盘条码数
                //WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_BarcodeLength, GlobalVar.gl_length_PCSBarcodeLength.ToString(), iniFilePath);   //制品条码长度
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SheetBarcodeLength, GlobalVar.gl_length_sheetBarcodeLength.ToString(), iniFilePath);   //Sheet制品条码长度
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_MarkPointDiameter, GlobalVar.gl_value_MarkPointDiameter.ToString(), iniFilePath);   //Sheet制品条码长度
                //WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_inikey_Line, GlobalVar.gl_LineName, iniFilePath);    //线别
                //WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_strDeviceID, GlobalVar.gl_DeviceID, iniFilePath);     //设备编号
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_iniKey_srcPicWidth, Global.GlobalVar.gl_SrcPictureWidth.ToString(), iniFilePath);   //制品条码长度
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_iniKey_srcPicHeight, Global.GlobalVar.gl_SrcPictureHeight.ToString(), iniFilePath);   //制品条码长度
                //結果文件存儲、上傳、備份位置
                //WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SaveResultPath, GlobalVar.gl_path_FileResult, iniFilePath);    //結果文件存儲位置
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SaveBackUpPath, GlobalVar.gl_path_FileBackUp, iniFilePath);    //結果文件備份位置
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SavePics, GlobalVar.gl_saveCapturePics.ToString(), iniFilePath);   //是否存儲圖片
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PicSavePath, GlobalVar.gl_PicsSavePath.ToString(), iniFilePath);   //存儲圖片路徑
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SaveNGPics, GlobalVar.gl_saveDecodeFailPics.ToString(), iniFilePath);   //是否存儲解析NG圖片
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_NGPicSavePath, GlobalVar.gl_NGPicsSavePath.ToString(), iniFilePath);   //解析NG圖片存儲路徑
                //記錄掃碼串口
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_ScanSerialPort, GlobalVar.gl_serialPort_Scan, iniFilePath);  //串口扫描
                //软件限位
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_X_P, GlobalVar.gl_PosLimit_X_P.ToString(), iniFilePath);  //X轴正向限位
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_X_N, GlobalVar.gl_PosLimit_X_N.ToString(), iniFilePath);  //X轴负向限位
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_Y_P, GlobalVar.gl_PosLimit_Y_P.ToString(), iniFilePath);  //Y轴正向限位
                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_Y_N, GlobalVar.gl_PosLimit_Y_N.ToString(), iniFilePath);  //Y轴负向限位

                WritePrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_strUseHalcon, GlobalVar.gl_bUseHalcon.ToString(), iniFilePath);    //使用Halcon解析
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_inikey_Gain, Global.GlobalVar.gl_bmpOptimi_gain.ToString(), iniFilePath);   //Gain
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_inikey_OffSet, Global.GlobalVar.gl_bmpOptimi_offset.ToString(), iniFilePath);   //offset

                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Mode, Global.GlobalVar.gl_iniKey_SaveMode, Global.GlobalVar.gl_saveMode.ToString(), iniFilePath);  //存储模式

                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Path, Global.GlobalVar.gl_iniKey_ResultReadPath, Global.GlobalVar.gl_Path_ResultRead, iniFilePath);     //电测数据读取路径
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Path, Global.GlobalVar.gl_iniKey_ResultWritePath, Global.GlobalVar.gl_Path_ResultWrite, iniFilePath);   //电测数据存储路径
                //WritePrivateProfileString(Global.GlobalVar.gl_inisection_Path, Global.GlobalVar.gl_iniKey_SourcePicReadPath, Global.GlobalVar.gl_Path_srcPicture, iniFilePath);   //图片读取路径

                WritePrivateProfileString(GlobalVar.gl_inisection_TestInfo, GlobalVar.gl_iniKey_LotNo, GlobalVar.gl_str_LotNo, iniFilePath);   //LotNo

                WritePrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerIP, GlobalVar.gl_MasterIPInfo._IP, iniFilePath);   //socketserver IP地址
                WritePrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerMAC, GlobalVar.gl_MasterIPInfo._MAC, iniFilePath);   //socketserver MAC地址
                WritePrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerPort, GlobalVar.gl_MasterIPInfo._WorkPort.ToString(), iniFilePath);   //socketserver 工作端口

                WritePrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_BlockWidth, GlobalVar.block_width.ToString(), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_BlockHeigh, GlobalVar.block_heigt.ToString(), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_WorkAreaWidth, GlobalVar.gl_workArea_width.ToString(), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_inikey_WorkAreaHeight, GlobalVar.gl_workArea_height.ToString(), iniFilePath);
                #endregion

                #region 写入曝光数据EXPOSURE.INI
                string iniFilePath_mark = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;
                //默认的Mark点曝光值和解析曝光值（默认的红灯白灯都是打开的）
                WritePrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Mark, GlobalVar.gl_exposure_Mark_default.ToString(), iniFilePath_mark);
                WritePrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Matrix, GlobalVar.gl_exposure_Matrix_default.ToString(), iniFilePath_mark);

                string iniFilePath_exposure = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniExposure_FileName;
                iniFilePath_exposure = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;//将光源配置写入到同一文件
                ////曝光参数OLD
                //WritePrivateProfileString(GlobalVar.gl_iniSection_AAC, GlobalVar.gl_iniKey_Mark, GlobalVar.gl_exposure_Mark_AAC.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_AAC, GlobalVar.gl_iniKey_Matrix, GlobalVar.gl_exposure_Matrix_AAC.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_ST, GlobalVar.gl_iniKey_Mark, GlobalVar.gl_exposure_Mark_ST.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_ST, GlobalVar.gl_iniKey_Matrix, GlobalVar.gl_exposure_Matrix_ST.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_GEORTEK, GlobalVar.gl_iniKey_Mark, GlobalVar.gl_exposure_Mark_Geortek.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_GEORTEK, GlobalVar.gl_iniKey_Matrix, GlobalVar.gl_exposure_Matrix_Geortek.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_KNOWLES, GlobalVar.gl_iniKey_Mark, GlobalVar.gl_exposure_Mark_Knowles.ToString(), iniFilePath_exposure);
                //WritePrivateProfileString(GlobalVar.gl_iniSection_KNOWLES, GlobalVar.gl_iniKey_Matrix, GlobalVar.gl_exposure_Matrix_Knowles.ToString(), iniFilePath_exposure);
                //曝光参数NEW
                if (GlobalVar.gl_Model_prodcutTypeMic != "")
                {
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeMic.ToUpper(), GlobalVar.ini_key_MExposure, GlobalVar.gl_Model_exposure.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeMic.ToUpper(), GlobalVar.ini_key_MRedLight, GlobalVar.gl_Model_redLight.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeMic.ToUpper(), GlobalVar.ini_key_MWhiteLight, GlobalVar.gl_Model_whiteLight.ToString(), iniFilePath_exposure);
                }

                if (GlobalVar.gl_Model_prodcutTypeProx != "")
                {
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeProx.ToUpper(), GlobalVar.ini_key_MExposure, GlobalVar.gl_Model_exposureProx.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeProx.ToUpper(), GlobalVar.ini_key_MRedLight, GlobalVar.gl_Model_redLight.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString(GlobalVar.gl_Model_prodcutTypeProx.ToUpper(), GlobalVar.ini_key_MWhiteLight, GlobalVar.gl_Model_whiteLight.ToString(), iniFilePath_exposure);
                }
                if (GlobalVar.gl_LinkType == LinkType.BARCODE)// barcode光源数据写入 [10/19/2017 617004]
                {
                    WritePrivateProfileString("BARCODE", GlobalVar.ini_key_MExposure, GlobalVar.gl_Model_exposurePcs.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString("BARCODE", GlobalVar.ini_key_MRedLight, GlobalVar.gl_Model_redLight.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString("BARCODE", GlobalVar.ini_key_MWhiteLight, GlobalVar.gl_Model_whiteLight.ToString(), iniFilePath_exposure);
                }
                if (GlobalVar.gl_LinkType == LinkType.IC)// IC光源数据写入 [01/11/2018 617004]
                {
                    WritePrivateProfileString("IC", GlobalVar.ini_key_MExposure, GlobalVar.gl_Model_exposureIC.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString("IC", GlobalVar.ini_key_MRedLight, GlobalVar.gl_Model_redLight.ToString(), iniFilePath_exposure);
                    WritePrivateProfileString("IC", GlobalVar.ini_key_MWhiteLight, GlobalVar.gl_Model_whiteLight.ToString(), iniFilePath_exposure);
                }
                #endregion
            }
            catch { }
        }

        public void WriteCalPositionInfoToTBS()
        {
            try
            {
                string iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;
                WritePrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_X, GlobalVar.gl_point_CalPos.Pos_X.ToString("00.000"), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_Y, GlobalVar.gl_point_CalPos.Pos_Y.ToString("00.000"), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CalibrateRatio_X, GlobalVar.gl_value_CalibrateRatio_X.ToString("0.000000000"), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CalibrateRatio_Y, GlobalVar.gl_value_CalibrateRatio_Y.ToString("0.000000000"), iniFilePath);
            }
            catch
            {
                MessageBox.Show("校準坐標點信息存儲失敗, 請重新存儲");
            }
        }

        public void WriteRefPositionInfoToTBS()
        {
            try
            {
                string iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;
                WritePrivateProfileString(GlobalVar.gl_ProductModel, GlobalVar.gl_iniKey_RefPoint_X, GlobalVar.gl_Ref_Point_Axis.Pos_X.ToString("00.000"), iniFilePath);
                WritePrivateProfileString(GlobalVar.gl_ProductModel, GlobalVar.gl_iniKey_RefPoint_Y, GlobalVar.gl_Ref_Point_Axis.Pos_Y.ToString("00.000"), iniFilePath);
            }
            catch
            {
                MessageBox.Show("校準坐標點信息存儲失敗, 請重新存儲");
            }
        }

        //读取TBS.INI配置信息
        public void ReadGlobalInfoFromTBS()
        {
            try
            {
                StringBuilder str_tmp = new StringBuilder(100);
                string iniFilePath = GlobalVar.gl_strTargetPath + "\\" + GlobalVar.gl_iniTBS_FileName;
                if (System.IO.File.Exists(iniFilePath))
                {
                    #region 测试参数配置/CONFIG.INI
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Mode, Global.GlobalVar.gl_iniKey_SaveMode, "", str_tmp, 50, iniFilePath);
                    //Global.GlobalVar.gl_saveMode = (str_tmp.ToString().Trim() == "") ? 0 : Convert.ToInt32(str_tmp.ToString().Trim());  //存储模式

                    GetPrivateProfileString(GlobalVar.gl_inisection_TestInfo, GlobalVar.gl_iniKey_LotNo, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_str_LotNo = str_tmp.ToString().Trim();  //LotNo

                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_MinMatchScore, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_MinMatchScore = (str_tmp.ToString().Trim() == "") ? 80 : Convert.ToInt32(str_tmp.ToString().Trim()); //匹配值
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_DecodeTimes, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_decode_times = (str_tmp.ToString().Trim() == "") ? 4 : Convert.ToInt32(str_tmp.ToString().Trim()); //解析超时
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_TimeOut, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_decode_timeout = (str_tmp.ToString().Trim() == "") ? 100000 : Convert.ToInt32(str_tmp.ToString().Trim()); //解析超时
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SheetCount, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_OneSheetCount = (str_tmp.ToString().Trim() == "") ? 48 : Convert.ToInt32(str_tmp.ToString().Trim());  //整盘条码数
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_BarcodeLength, "", str_tmp, 50, iniFilePath);
                    //GlobalVar.gl_length_PCSBarcodeLength = (str_tmp.ToString().Trim() == "") ? 17 : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SheetBarcodeLength, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_length_sheetBarcodeLength = (str_tmp.ToString().Trim() == "") ? 11 : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_MarkPointDiameter, "", str_tmp, 50, iniFilePath); //Mark點直徑
                    try
                    {
                        GlobalVar.gl_value_MarkPointDiameter = (str_tmp.ToString().Trim() == "") ? 1.0f : float.Parse(str_tmp.ToString().Trim());
                    }
                    catch { GlobalVar.gl_value_MarkPointDiameter = 1.0f; }

                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_strUseHalcon, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_bUseHalcon = Convert.ToBoolean(str_tmp.ToString().Trim() == "" ? "False" : "True"); //使用Halcon解析

                    //文件存儲位置，包括結果文檔存儲、備份位置。
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SaveBackUpPath, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_path_FileBackUp = str_tmp.ToString().Trim();
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SavePics, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_saveCapturePics = (str_tmp.ToString().Trim() == "") ? false : Convert.ToBoolean(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PicSavePath, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_PicsSavePath = str_tmp.ToString().Trim();
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_SaveNGPics, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_saveDecodeFailPics = (str_tmp.ToString().Trim() == "") ? false : Convert.ToBoolean(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_NGPicSavePath, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_NGPicsSavePath = str_tmp.ToString().Trim();
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_ScanSerialPort, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_serialPort_Scan = str_tmp.ToString();
                    //软件限位
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_X_P, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_PosLimit_X_P = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_PosLimit_X_P : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_X_N, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_PosLimit_X_N = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_PosLimit_X_N : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_Y_P, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_PosLimit_Y_P = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_PosLimit_Y_P : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_Global, GlobalVar.gl_iniKey_PosLimit_Y_N, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_PosLimit_Y_N = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_PosLimit_Y_N : Convert.ToInt32(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_iniKey_srcPicWidth, "", str_tmp, 50, iniFilePath);
                    //Global.GlobalVar.gl_SrcPictureWidth = (str_tmp.ToString().Trim() == "") ? 640 : Convert.ToInt32(str_tmp.ToString().Trim());  
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_iniKey_srcPicHeight, "", str_tmp, 50, iniFilePath);
                    //Global.GlobalVar.gl_SrcPictureHeight = (str_tmp.ToString().Trim() == "") ? 480 : Convert.ToInt32(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_inikey_Gain, "", str_tmp, 50, iniFilePath);  //Gain
                    //Global.GlobalVar.gl_bmpOptimi_gain = (str_tmp.ToString().Trim() == "") ? Global.GlobalVar.gl_bmpOptimi_gain : float.Parse(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_inikey_OffSet, "", str_tmp, 50, iniFilePath);  //OffSet
                    //Global.GlobalVar.gl_bmpOptimi_offset = (str_tmp.ToString().Trim() == "") ? Global.GlobalVar.gl_bmpOptimi_offset : Convert.ToInt32(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(Global.GlobalVar.gl_inisection_Global, Global.GlobalVar.gl_inikey_ShapeMatch, "", str_tmp, 50, iniFilePath);  //OffSet
                    //Global.GlobalVar.gl_ShapeAnalysis = (str_tmp.ToString().Trim() == "") ? Global.GlobalVar.gl_ShapeAnalysis : Convert.ToBoolean(str_tmp.ToString().Trim()); 

                    //测试NG信息统计
                    GetPrivateProfileString(GlobalVar.gl_inisection_TestInfo, GlobalVar.gl_iniKey_TotalSheets, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_testinfo_totalSheet = (str_tmp.ToString().Trim() == "") ? 0 : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_TestInfo, GlobalVar.gl_iniKey_TotalDecodeFailed, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_testinfo_decodefailed = (str_tmp.ToString().Trim() == "") ? 0 : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_inisection_TestInfo, GlobalVar.gl_iniKey_TotalTest, "", str_tmp, 100, iniFilePath);
                    GlobalVar.gl_testinfo_totalTest = (str_tmp.ToString().Trim() == "") ? 0 : Convert.ToInt32(str_tmp.ToString().Trim());

                    //ltt
                    //GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_X, "", str_tmp, 100, iniFilePath);
                    //GlobalVar.gl_point_CalPos.Pos_X = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_Y, "", str_tmp, 100, iniFilePath);
                    //GlobalVar.gl_point_CalPos.Pos_Y = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim());

                    GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CalibrateRatio_X, "", str_tmp, 100, iniFilePath);
                    try { GlobalVar.gl_value_CalibrateRatio_X = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim()); }
                    catch { GlobalVar.gl_value_CalibrateRatio_X = 0.0f; }
                    GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CalibrateRatio_Y, "", str_tmp, 100, iniFilePath);
                    try { GlobalVar.gl_value_CalibrateRatio_Y = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim()); }
                    catch { GlobalVar.gl_value_CalibrateRatio_Y = 0.0f; }

                    GetPrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerIP, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_MasterIPInfo._IP = str_tmp.ToString().Trim();  //socketserver IP地址
                    GetPrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerMAC, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_MasterIPInfo._MAC = str_tmp.ToString().Trim();  //socketserver MAC地址
                    GetPrivateProfileString(GlobalVar.gl_inisection_SocketServerInfo, GlobalVar.gl_iniKey_SocketServerPort, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_MasterIPInfo._WorkPort = (str_tmp.ToString().Trim() == "") ? 0 : Convert.ToInt32(str_tmp.ToString().Trim());  //socketserver 工作端口

                    GetPrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_BlockWidth, "", str_tmp, 50, iniFilePath);
                    GlobalVar.block_width = (str_tmp.ToString().Trim() == "") ? GlobalVar.block_width : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_BlockHeigh, "", str_tmp, 50, iniFilePath);
                    GlobalVar.block_heigt = (str_tmp.ToString().Trim() == "") ? GlobalVar.block_width : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_iniKey_WorkAreaWidth, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_workArea_width = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_workArea_width : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_Size, GlobalVar.gl_inikey_WorkAreaHeight, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_workArea_height = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_workArea_height : Convert.ToInt32(str_tmp.ToString().Trim());
                    //默认ODBC连接
                    GetPrivateProfileString("ODBC", "DefaultODBC", "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_strDefaultODBC = str_tmp.ToString().Trim() == "" ? "EBSFLIB" : str_tmp.ToString().Trim();
                    WritePrivateProfileString("ODBC", "DefaultODBC", GlobalVar.gl_strDefaultODBC.ToString(), iniFilePath);
                    #endregion
                }
                iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniExposure_FileName;
                iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;//统一到config文件中
                if (System.IO.File.Exists(iniFilePath))
                {
                    //default
                    //GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                    //GlobalVar.gl_exposure_Mark_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_default : Convert.ToInt32(str_tmp.ToString().Trim());
                    //GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                    //GlobalVar.gl_exposure_Matrix_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_default : Convert.ToInt32(str_tmp.ToString().Trim());
                    //AAC
                    GetPrivateProfileString(GlobalVar.gl_iniSection_AAC, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Mark_AAC = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_AAC : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_AAC, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Matrix_AAC = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_AAC : Convert.ToInt32(str_tmp.ToString().Trim());
                    //ST
                    GetPrivateProfileString(GlobalVar.gl_iniSection_ST, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Mark_ST = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_ST : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_ST, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Matrix_ST = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_ST : Convert.ToInt32(str_tmp.ToString().Trim());
                    //KNOWLES
                    GetPrivateProfileString(GlobalVar.gl_iniSection_KNOWLES, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Mark_Knowles = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_Knowles : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_KNOWLES, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Matrix_Knowles = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_Knowles : Convert.ToInt32(str_tmp.ToString().Trim());
                    //GEORTEK
                    GetPrivateProfileString(GlobalVar.gl_iniSection_GEORTEK, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Mark_Geortek = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_Geortek : Convert.ToInt32(str_tmp.ToString().Trim());
                    GetPrivateProfileString(GlobalVar.gl_iniSection_GEORTEK, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                    GlobalVar.gl_exposure_Matrix_Geortek = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_Geortek : Convert.ToInt32(str_tmp.ToString().Trim());

                }
            }
            catch { }
        }

        /// <summary>
        /// 读取品目后，读取曝光值配置信息 2017.04.24
        /// </summary>
        /// <param name="productType">没有配置取默认值</param>
        public void ReadProductTypeExposure(string productType)
        {
            try
            {
                StringBuilder str_tmp = new StringBuilder(100);
                string iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniExposure_FileName;
                iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;//统一到config文件中
                if (!System.IO.File.Exists(iniFilePath)) { return; }
                ////default
                //GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                //GlobalVar.gl_exposure_Mark_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_default : Convert.ToInt32(str_tmp.ToString().Trim());
                //GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                //GlobalVar.gl_exposure_Matrix_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_default : Convert.ToInt32(str_tmp.ToString().Trim());

                //曝光值NEW
                GetPrivateProfileString(productType, GlobalVar.ini_key_MExposure, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_Model_exposure = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_default : Convert.ToInt32(str_tmp.ToString().Trim());
                GetPrivateProfileString(productType, GlobalVar.ini_key_MRedLight, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_Model_redLight = (str_tmp.ToString().Trim() == "") ? 1 : Convert.ToInt32(str_tmp.ToString().Trim());
                GetPrivateProfileString(productType, GlobalVar.ini_key_MWhiteLight, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_Model_whiteLight = (str_tmp.ToString().Trim() == "") ? 1 : Convert.ToInt32(str_tmp.ToString().Trim());
            }
            catch { }
        }

        public void ReadMarkDefault()
        {
            try
            {
                StringBuilder str_tmp = new StringBuilder(100);
                string iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;
                if (!System.IO.File.Exists(iniFilePath)) { return; }
                //default mark点曝光值
                GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Mark, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_exposure_Mark_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Mark_default : Convert.ToInt32(str_tmp.ToString().Trim());
                GetPrivateProfileString(GlobalVar.gl_iniSection_Default, GlobalVar.gl_iniKey_Matrix, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_exposure_Matrix_default = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_exposure_Matrix_default : Convert.ToInt32(str_tmp.ToString().Trim());
                //参考点位置
                GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_X, "", str_tmp, 100, iniFilePath);
                GlobalVar.gl_point_CalPos.Pos_X = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_point_CalPosRef.Pos_X : float.Parse(str_tmp.ToString().Trim());
                GetPrivateProfileString(GlobalVar.gl_iniSection_CALPosition, GlobalVar.gl_iniKey_CALPos_Y, "", str_tmp, 100, iniFilePath);
                GlobalVar.gl_point_CalPos.Pos_Y = (str_tmp.ToString().Trim() == "") ? GlobalVar.gl_point_CalPosRef.Pos_Y : float.Parse(str_tmp.ToString().Trim());
            }
            catch { }
        }

        //读取品目信息后，读取参考点配置信息
        public void ReadRefPointInfoFromTBS()
        {
            try
            {
                StringBuilder str_tmp = new StringBuilder(100);
                string iniFilePath = Application.StartupPath + "\\" + GlobalVar.gl_ProductModel + "\\" + GlobalVar.gl_LinkType + "\\" + GlobalVar.gl_iniTBS_FileName;
                if (!System.IO.File.Exists(iniFilePath)) { return; }

                //参考坐标
                GetPrivateProfileString(GlobalVar.gl_ProductModel, GlobalVar.gl_iniKey_RefPoint_X, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_Ref_Point_Axis.Pos_X = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim());
                GetPrivateProfileString(GlobalVar.gl_ProductModel, GlobalVar.gl_iniKey_RefPoint_Y, "", str_tmp, 50, iniFilePath);
                GlobalVar.gl_Ref_Point_Axis.Pos_Y = (str_tmp.ToString().Trim() == "") ? 0.0f : float.Parse(str_tmp.ToString().Trim());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Application.Exit();
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        private static void DeleteFolder()
        {
            string vBatFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\DeleteItself.bat";
            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
            {
                vStreamWriter.Write(string.Format(
                    ":del\r\n" +
                    " del \"{0}\"\r\n" +
                    "if exist \"{0}\" goto del\r\n" +
                    "del %0\r\n", Application.ExecutablePath));
            }

            //************ 执行批处理
            WinExec(vBatFile, 0);
            //************ 结束退出
            Application.Exit();
        }
        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        public static void createNewLnk()
        {
            string deskTop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//桌面文件夹
            if (System.IO.File.Exists(deskTop + "\\条码照合关联程序3型.lnk"))
            {
                System.IO.File.Delete(deskTop + "\\条码照合关联程序3型.lnk");
            }
            WshShell shell = new WshShell();//快捷键方式创建的位置、名称
            IWshShortcut shortcut = shell.CreateShortcut(deskTop + "\\条码照合关联程序3型.lnk");
            //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + "条码照合程序3型.lnk");
            shortcut.TargetPath = GlobalVar.gl_strAppPath + "条码照合关联程序3型.exe"; //目标文件
            shortcut.WorkingDirectory = GlobalVar.gl_strAppPath;//该属性指定应用程序的工作目录，当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。
            shortcut.Description = ""; //描述
            shortcut.IconLocation = Application.StartupPath + "\\条码照合关联程序3型.exe";  //快捷方式图标
            shortcut.Arguments = "";
            //shortcut.Hotkey = "CTRL+ALT+F11"; // 快捷键
            shortcut.Save(); //必须调用保存快捷才成创建成功   
        }

        /// <summary>
        /// 递归删除文件夹下的文件（包括子文件夹）
        /// </summary>
        /// <param name="strP"></param>
        public static void DeleteLogFunc(string strP, int day)
        {
            try
            {
                if (day <= 0) day = 15;
                DirectoryInfo d = new DirectoryInfo(strP);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    if (fsinfo is DirectoryInfo)     //判断是否为文件夹  
                    {
                        DeleteLogFunc(fsinfo.FullName, day);//递归调用
                    }
                    else
                    {
                        TimeSpan ts = DateTime.Now.Subtract(fsinfo.CreationTime);
                        if (ts.Days > day)
                        {
                            System.IO.File.Delete(fsinfo.FullName);
                        }
                    }
                }
            }
            catch { }
        }

    }
}