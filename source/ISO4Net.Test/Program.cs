


using ISO4Net.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenTrx.NET {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("*** iso4Net Test ***");
            Console.WriteLine("-------------------");

            Console.WriteLine("Reading packager config...");
            string cfgFile = "..\\..\\..\\ISO4Net.Library\\Packagers\\iso87_ascii.xml";

            GenericPackager p = new GenericPackager(cfgFile);
            p.HeaderLength = 3;     // "ISO" word
            p.HeaderASCII = true;

            // *** Authorization example created from scratch, based on the GenericPackager 'p' ***
            ISOMessage request = p.CreateISOMessage();
            request.Header = new ISOHeader(ASCIIEncoding.ASCII.GetBytes("ISO"));
            request.MTI = "0200";
            request.Add(2, "4444333322221111");
            request.Add(3, "000000");
            request.Add(4, "1000");
            request.Add(7, "0428180300");
            request.Add(11, "000099");
            request.Add(12, "150300");
            request.Add(13, "0428");
            request.Add(14, "1512");
            request.Add(22, "021");
            request.Add(41, "12345678");
            request.Add(42, "88991740");
            request.Add(49, "840");
            request.Add(52, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            request.Add(64, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            request.Add(70, "123");
            request.Add(128, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

            Console.WriteLine("Encoding 0200 message...");
            byte[] packed200 = request.Encode();
            Console.WriteLine("Encoded HEX:");
            Console.WriteLine(Utils.HexString(packed200));

            Console.WriteLine("\nDump:");
            Console.WriteLine(request.Dump(true));

            Console.WriteLine("\nDecoding back to ISOMessage object:");
            ISOMessage response = new ISOMessage();
            p.Decode(response, packed200);
            Console.WriteLine("Decoding result:");
            Console.WriteLine(response.Dump(true));

        }
    }
}
