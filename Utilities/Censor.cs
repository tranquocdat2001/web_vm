using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    public class Censor
    {
        public IList<string> CensoredWords { get; private set; }

        public Censor(IEnumerable<string> censoredWords)
        {
            if (censoredWords == null)
                throw new ArgumentNullException("censoredWords");

            CensoredWords = new List<string>(censoredWords);
        }

        public string CensorText(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            string censoredText = text;

            foreach (string censoredWord in CensoredWords)
            {
                string regularExpression = ToRegexPattern(censoredWord);

                censoredText = Regex.Replace(censoredText, regularExpression, StarCensoredMatch,
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return censoredText;
        }

        private static string StarCensoredMatch(Match m)
        {
            string word = m.Captures[0].Value;

            return new string('*', word.Length);
        }

        private string ToRegexPattern(string wildcardSearch)
        {
            string regexPattern = Regex.Escape(wildcardSearch);

            regexPattern = regexPattern.Replace(@"\*", ".*?");
            regexPattern = regexPattern.Replace(@"\?", ".");

            if (regexPattern.StartsWith(".*?"))
            {
                regexPattern = regexPattern.Substring(3);
                regexPattern = @"(^\b)*?" + regexPattern;
            }

            regexPattern = @"\b" + regexPattern + @"\b";

            return regexPattern;
        }

        public static string FilterWord
        {
            get
            {
                try
                {
                    return System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/badword.txt"));
                }
                catch
                {
                    return "đụ|vkl|vcl|vãi lều|fuck|shit|ifastnet|sitesled|buồi|nứng|lồn|fucking|cock|bitch|cặc|kặc|suck|minh râu|việt cộng|triều đình cộng sản|vương triều cộng sản|vương triều Hà Nội|đảng trị|đả đảo cộng sản|đả đảo XHCN|cộng sản bán nước|tội ác cộng sản|tội ác của chế độ CSVN|tội ác của chế độ cộng sản|cộng sản độc tài|độc tài cộng sản|cộng sản thối nát|cộng sản mị dân|cộng sản mỵ dân|địt mẹ mày|lồn mẹ|lỗ đít|ăn lồn|liếm lìn|mặt lìn|con phò|con kặc|Minh Râu|lịt mẹ|tiên sư bố|tổ sư bố|mả mẹ mày|mả cha mày|mả bố mày|mả tổ mày|mả mẹ chúng mày|mả cha chúng mày|mả bố chúng mày|mả tổ chúng mày|lỗ đít|mặt lìn|liếm lìn|móc lìn|ăn lìn|liếm đít|con phò|con đĩ|con điếm|đĩ đực|điếm đực|đồ chó|đồ lợn|cứt|ăn kặc|bú kặc|củ kặc|con kặc|mút kặc|ăn kẹc|bú kẹc|củ kẹc|con kẹc|bú dái|bú dzái|mút zái|bú cu|bú ku|con ku|kứt|ngứa dzái|ngứa dái|bán dzâm|ngứa zdái|bỏ mẹ|ĐỴT|dis mẹ|nhu lon|lon`|zú bự|zu' bu|zu' bự|Minh Trị|Minh trị|minh trị|MINH TRỊ|CỜ VÀNG 3 SỌC ĐỎ|Cờ Vàng 3 Sọc Đỏ|cờ vàng ba sọc đỏ|đàn áp tôn giáo|ĐÀN ÁP TÔN GIÁO|Đàn Áp Tôn Giáo|thăng tiến việt nam|Cu Hồ|hot^. le.|hột le|Trung cộng|Viêt Cộng|TRung Cong|Viet COng|TRung COng|Thanh Minh Thiền viện|trYm|BUỒI|hãm|CPVNTD|sjp|BUỒI|LO^`N|BUÔ`i|trYm|cặc|f.u.c.k|F.U.C.K|f u c k|F U C K|dY~|F*ck|địt|ĐỴT|đít ghẻ|chó đẻ|fản động|phản động|Đỵt mẸ|ĐỴT|đéo|địt tổ|đỵt mẹ|Đỵt mẹ|đis mẹ|đIs mẹ|đỤ mẸ|DIT ME|Đy~ chó|Đĩ chó|đù má|đéo mẹ|địt mẹ mày|đù má mày|đéo mẹ mày|địt mẹ chúng mày|đù má chúng mày|đéo mẹ chúng mày|đéo mẹ|địt|địt mẹ|lỳn|kon kac|con di~|thang con ket|lo`n|chó ghẻ|lonz";
                }
            }
        }

        public static List<string> LstBadWord = FilterWord.Split('|').ToList();
        public static string FilterWordPattern = string.Format("(^|\\W)({0})(\\W|$)", FilterWord);
    }
}
