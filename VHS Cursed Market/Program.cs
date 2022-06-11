using CranchyLib.Networking;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VHS_Cursed_Market
{
    class Program
    {
        private static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.Write("Press Enter To Continue...");

            Console.ReadLine();
            FiddlerCore.Stop();
            Environment.Exit(0);
        }
        private static void ObtainMarketFile()
        {
            Console.WriteLine("[GET] https://dbd.cranchpalace.info/market/vhs/market.json");
            var request = Networking.Request.Get("https://dbd.cranchpalace.info/market/vhs/market.json", new string[] { });
            if (request.Item1 == Networking.E_StatusCode.OK)
            {
                Globals.marketFile = request.Item3;
                Console.WriteLine("[+] Cursed Market -> Market File Successfully Obtained!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("[-] Failed To Obtain Market File!");
                PressEnterToContinue();
            }
        }
        private static void PerformVersionCheck()
        {
            Networking.CreateNewWebProxyInstance();
            try
            {
                Networking.Request.Get($"https://dbd.cranchpalace.info/market/vhs/heartBeat", new string[] { });
            }
            catch (NullReferenceException e)
            {
                Messaging.ShowMessage("Something Is Wrong With Ethernet Connection! Cursed Market Failed To Connect To It's Server, Sometimes Reason Is Ethernet Provider, Which Restrictions You Can Bypass Using VPN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string[] headers =
            {
                "User-Agent: Cursed Market VHS"
            };
            Console.WriteLine($"[GET] https://dbd.cranchpalace.info/market/vhs/versionCheck?version={Globals.OfflineVersion}");
            var request = Networking.Request.Get($"https://dbd.cranchpalace.info/market/vhs/versionCheck?version={Globals.OfflineVersion}", headers);
            if (request.Item1 != Networking.E_StatusCode.OK)
            {
                if (Messaging.ShowDialog($"Something Went Wrong When Cursed Market Tried To Check Latest Client Version!\nStatus Code: {request.Item1} [{(int)request.Item1}]\n\nShall Cursed Market Try To Fix The Issue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FiddlerCore.Stop();
                    WinReg.DisableProxy();
                    Application.Restart();
                }
                else
                {
                    PressEnterToContinue();
                }
            }


            if (request.Item3.IsJson() == false)
            {
                Messaging.ShowMessage("Cursed Market Failed To Parse Server Response!\nReason: Invalid JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            JObject json = JObject.Parse(request.Item3);
            Console.WriteLine("[+] SERVERNAME Discord Server: " + json["Discord"]);
            if ((bool)json["isLatest"] == false)
            {
                if (Messaging.ShowDialog($"New Version Of Cursed Market is Available! Download It?\nCurrent Version: {Globals.OfflineVersion}\nLatest Version: {json["onlineVersion"]}\n\nSave To: {Networking.Utilities.Windows.SE_WinFolder.Downloads}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var download = Networking.Request.Download((string)json["Download"], Networking.Utilities.Windows.SE_WinFolder.Downloads);
                    if (download.Item1 == false)
                    {
                        if (Messaging.ShowDialog("Failed To Download Latest Cursed Market Version!\nShall We Try To Use Legacy Download Method?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Process.Start((string)json["Download"]);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Process.Start(download.Item2);
                        Environment.Exit(0);
                    }
                }
            }
            Console.WriteLine("[+] Cursed Market Version Successfully Checked!");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(" ██████╗██╗   ██╗██████╗ ███████╗███████╗██████╗     ███╗   ███╗ █████╗ ██████╗ ██╗  ██╗███████╗████████╗\n" +
                              "██╔════╝██║   ██║██╔══██╗██╔════╝██╔════╝██╔══██╗    ████╗ ████║██╔══██╗██╔══██╗██║ ██╔╝██╔════╝╚══██╔══╝\n" +
                              "██║     ██║   ██║██████╔╝███████╗█████╗  ██║  ██║    ██╔████╔██║███████║██████╔╝█████╔╝ █████╗     ██║   \n" +
                              "██║     ██║   ██║██╔══██╗╚════██║██╔══╝  ██║  ██║    ██║╚██╔╝██║██╔══██║██╔══██╗██╔═██╗ ██╔══╝     ██║   \n" +
                              "╚██████╗╚██████╔╝██║  ██║███████║███████╗██████╔╝    ██║ ╚═╝ ██║██║  ██║██║  ██║██║  ██╗███████╗   ██║   \n" +
                              " ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝╚══════╝╚═════╝     ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝   ╚═╝   \n" +
                              "                                                                                                         \n" +
                              "            ██╗   ██╗██╗  ██╗███████╗    ███████╗██████╗ ██╗████████╗██╗ ██████╗ ███╗   ██╗              \n" +
                              "            ██║   ██║██║  ██║██╔════╝    ██╔════╝██╔══██╗██║╚══██╔══╝██║██╔═══██╗████╗  ██║              \n" +
                              "            ██║   ██║███████║███████╗    █████╗  ██║  ██║██║   ██║   ██║██║   ██║██╔██╗ ██║              \n" +
                              "            ╚██╗ ██╔╝██╔══██║╚════██║    ██╔══╝  ██║  ██║██║   ██║   ██║██║   ██║██║╚██╗██║              \n" +
                              "             ╚████╔╝ ██║  ██║███████║    ███████╗██████╔╝██║   ██║   ██║╚██████╔╝██║ ╚████║              \n" +
                              "              ╚═══╝  ╚═╝  ╚═╝╚══════╝    ╚══════╝╚═════╝ ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝              \n" +
                              "                                                                                                         \n" +
                              "                  OPEN SOURCE, FREE TO USE PROJECT BY CRANCH THE WOLF - SERVERNAME 2022                  \n\n\n");


            Globals.EnsureSelfDataFolderExists();

            Console.WriteLine($"[+] Program Has Been Initialized, Current Version: {Globals.OfflineVersion[0]}.{Globals.OfflineVersion[1]}.{Globals.OfflineVersion[2]}.{Globals.OfflineVersion[3]}");
            Console.WriteLine("[!] Looking For Updates...");
            Console.WriteLine();

            PerformVersionCheck();
            ObtainMarketFile();

            if (FiddlerCore.Start())
            {
                Console.WriteLine("[+] FiddlerCore is Running, You Can Launch VHS Now!");
                Console.WriteLine("[!] Press ENTER to Disable Fiddler Proxy & Exit from Cursed Market");
                PressEnterToContinue();
            }

        }
    }
}
