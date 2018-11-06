using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PleaseGoogleHomeSkill_Csharp
{
    public static class Phrase
    {
        private static string skillName = "googleさん";

        private static string[] supportPhraseAfterSkillName =
        {
            "(で|に)聞いて",
            "(で|に)開いて",
            "(で|に)確認して",
        };
        private static string[] connectiveAfterAction =
        {
            "を"
        };

        private static string[] connectiveAfterSkillName =
        {
            "で",
            "を使って",
            "使って",
            "に",
            "の",
        };
        private static string[] supportPhraseAfterAction =
        {
            "を開始して",
            "をつけて",
            "を調べて",
            "を教えて",
            "を探して",
            "を検索して",
            "をサーチ",
            "を聞いて",
            "をチェックして",
            "を教えて",
            "を確認して",
            "をチェック",
            "を見て",
            "はどう",
            "はどうですか",
            "を調べて",
        };

        private static string[] invokePhraseAfterSkillName =
        {
            "を起動して",
            "を実行して",
            "をスタートして",
            "開いて",
            "を開いて",
            "起動して",
            "を呼び出して",
            "実行して",
            "スタートして",
            "呼び出して",
            "で",
            "に",
        };



        static bool IsValidSkillCallFormat(string phrase)
        {

            return false;
        }

        public static string ComposeAskGoogleHomeText(string phrase)
        {
            //phraseからは空白を削除しよう
            phrase = Regex.Replace(phrase, @"\s+", "");

            string speechText = "";

            var startWithSkillNamePattern = @"^google\s*さん";

            if (Regex.IsMatch(phrase, startWithSkillNamePattern))
            {//呼び出し名から始まるパターン

                //呼び出し名を削除
                speechText = Regex.Replace(phrase, startWithSkillNamePattern, "");

                //つなぎ語が先頭に来るはず
                //先頭がつなぎ語かチェック
                var connective = connectiveAfterSkillName.OrderByDescending(item => item.Length)
                    .FirstOrDefault(item => Regex.IsMatch(speechText, $"^{item}"));
                if (connective != null)
                {
                    speechText = Regex.Replace(speechText, $"^{connective}", "");
                }

                //起動フレーズが来るかも
                //来たら削除
                var invokePhrase = invokePhraseAfterSkillName.OrderByDescending(item => item.Length)
                    .FirstOrDefault(item => Regex.IsMatch(speechText, $"^{item}"));
                if (invokePhrase != null)
                {
                    speechText = Regex.Replace(speechText, $"{invokePhrase}", "");
                }

                //「聞いて」をそのままGoogleへの問いかけに使うと変になるので
                speechText = Regex.Replace(speechText, "聞いて", "教えて");
                speechText = $"ねえ、グーグルさん。{speechText}";
            }
            else
            {//呼び出し名から始まらないパターン

                //そもそも呼び出し名が含まれているかどうか
                if (phrase.Contains(skillName))
                {
                    //呼び出し名が含まれている
                    //そのパターンは以下のとおりであることを前提とする。
                    //アクション　+　つなぎ語　+　呼び出し名　+　サポートフレーズ

                    //呼び出し名からみて、後ろがサポートフレーズ
                    //前が「アクション　+　つなぎ語」
                    var splittedPhrase = Regex.Split(phrase, skillName);
                    var frontPhrase = splittedPhrase[0];
                    var rearPhrase = splittedPhrase[1];


                    //後半部がサポートフレーズから始まっているかどうか
                    var supportPhrase = supportPhraseAfterSkillName.OrderByDescending(item => item.Length)
                        .FirstOrDefault(item => Regex.IsMatch(rearPhrase, $"^{item}"));
                    if (supportPhrase == null)
                    {
                        //例外
                        speechText = "";
                        return speechText;
                    }


                    ////前半部分からつなぎ語を削除したものがアクション
                    //var endWithSkillNamePattern = @"google\s*さん$";

                    //つなぎ語を削除しない
                    var connective = connectiveAfterAction.OrderByDescending(item => item.Length)
                        .FirstOrDefault(item => Regex.IsMatch(frontPhrase, $"{item}$"));
                    if (connective == null)
                    {
                        //例外
                        speechText = "";
                        return speechText;
                    }

                    //代わりにサポートフレーズ側のつなぎ語を削除
                    //この場合は先頭の１文字（で、に）
                    rearPhrase = rearPhrase.Substring(1);

                    //聞いての場合はそのままGoogleへの問いかけに渡すと変になる
                    if (rearPhrase == "聞いて")
                    {
                        rearPhrase = "教えて";
                    }
                    speechText = $"ねえ、ぐーぐるさん。{frontPhrase}{rearPhrase}";
                }
                else
                {
                    //呼び出し名が含まれていない場合はそのままグーグルさんに渡す。
                    //と思ったけれど、
                    //「聞いて」をそのままGoogleへの問いかけに使うと変になるので
                    speechText = Regex.Replace(phrase, "聞いて", "教えて");
                    speechText = $"ねえ、グーグルさん。{speechText}";
                }




            }

            return speechText;
        }
    }

}
