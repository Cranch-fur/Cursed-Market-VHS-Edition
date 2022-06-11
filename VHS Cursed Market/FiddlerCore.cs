using Fiddler;
using System.IO;

namespace VHS_Cursed_Market
{
    public static class FiddlerCore
    {
        public static string rootCertificatePath = $"{Globals.SelfDataFolder}\\CursedMarket_RootCertificate.p12";
        public const string rootCertificatePassword = "F035d044eb43607be11122518ca60f38";

        static FiddlerCore()
        {
            FiddlerApplication.BeforeRequest += FiddlerToCatchBeforeRequest;
            FiddlerApplication.ResetSessionCounter();
        }
        private static bool EnsureRootCertificate()
        {
            BCCertMaker.BCCertMaker certProvider = new BCCertMaker.BCCertMaker();
            CertMaker.oCertProvider = certProvider;

            if (!File.Exists(rootCertificatePath))
            {
                certProvider.CreateRootCertificate();
                certProvider.WriteRootCertificateAndPrivateKeyToPkcs12File(rootCertificatePath, rootCertificatePassword);
            }
            else certProvider.ReadRootCertificateAndPrivateKeyFromPkcs12File(rootCertificatePath, rootCertificatePassword);

            if (!CertMaker.rootCertIsTrusted())
            {
                CertMaker.trustRootCert();
            }

            return true;

        }
        public static bool DestroyRootCertificates()
        {
            CertMaker.removeFiddlerGeneratedCerts(true);
            return true;
        }



        public static bool Start()
        {
            if (EnsureRootCertificate())
                FiddlerApplication.Startup(new FiddlerCoreStartupSettingsBuilder().ListenOnPort(8866).RegisterAsSystemProxy().ChainToUpstreamGateway().DecryptSSL().OptimizeThreadPool().Build());
            return true;
        }
        public static void Stop()
        {
            FiddlerApplication.Shutdown();
        }
        public static bool GetIsRunning() { return FiddlerApplication.IsStarted(); }



        public static void FiddlerToCatchBeforeRequest(Session oSession)
        {
            if (oSession.hostname == "api.vhsgame.com")
            {
                if (oSession.uriContains("/metagame/THEEND_Game/Client/Discover/?guid="))
                {
                    if (Globals.marketFile != null)
                    {
                        oSession.utilCreateResponseAndBypassServer();
                        oSession.utilSetResponseBody(Globals.marketFile);

                        return;
                    }
                }
            }
        }
    }
}
