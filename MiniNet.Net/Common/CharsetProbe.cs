using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.Net.Common
{
    /// <summary>
    /// 分析字节流是否是UTF-8编码。
    /// </summary>
    public static class CharsetProbe
    {
        public static bool IsValidUtf8(byte[] rawtext)
        {
            int score = 0;
            int i, rawtextlen = 0;
            int goodbytes = 0, asciibytes = 0;

            try
            {
                // Maybe also use UTF8 Byte Order Mark:  EF BB BF
                if (rawtext[0].Equals(239) && rawtext[1].Equals(187) && rawtext[2].Equals(191))
                {
                    return true;
                }

                // Check to see if characters fit into acceptable ranges
                rawtextlen = rawtext.Length;
                for (i = 0; i < rawtextlen; i++)
                {
                    if ((rawtext[i] & (byte)0x7F) == rawtext[i])
                    {  // One byte
                        asciibytes++;
                        // Ignore ASCII, can throw off count
                    }
                    else
                    {
                        //int m_rawInt0 = Convert.ToInt16(rawtext[i]);
                        //int m_rawInt1 = Convert.ToInt16(rawtext[i + 1]);
                        //int m_rawInt2 = Convert.ToInt16(rawtext[i + 2]);

                        //256-64  256-33

                        if (192 <= rawtext[i] && rawtext[i] <= 223 && // Two bytes
                            i + 1 < rawtextlen &&
                            128 <= rawtext[i + 1] && rawtext[i + 1] <= 191)
                        {
                            //256-128   256-65

                            goodbytes += 2;
                            i++;
                        }
                        else if (224 <= rawtext[i] && rawtext[i] <= 239 && // Three bytes
                            i + 2 < rawtextlen &&
                            128 <= rawtext[i + 1] && rawtext[i + 1] <= 191 &&
                            128 <= rawtext[i + 2] && rawtext[i + 2] <= 191)
                        {

                            goodbytes += 3;
                            i += 2;
                        }
                    }
                }

                if (asciibytes == rawtextlen)
                {
                    return false;
                }

                score = (int)(100 * ((float)goodbytes / (float)(rawtextlen - asciibytes)));

                // If not above 98, reduce to zero to prevent coincidental matches
                // Allows for some (few) bad formed sequences
                if (score > 98)
                {
                    return true;
                }
                else if (score > 95 && goodbytes > 30)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
