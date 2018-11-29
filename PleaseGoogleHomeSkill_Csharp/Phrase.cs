using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PleaseGoogleHomeSkill_Csharp
{
    public static class Phrase
    {
        private readonly static string askPattern = "(聞|訊|き)いて";



        public static string ComposeAskSmartSpeakerText(string phrase)
        { 

            //下準備：phraseからは空白を削除しよう
            phrase = Regex.Replace(phrase, @"\s+", "");


            string speechText = "";



            var startWithSkillNamePattern = @"^(ねえ)?.*?(google)\s*(さん)?";

            //呼び出し名によってスキルが起動された後、ユーザーが
            //「グーグルさんに～を聞いて」といった場合。
            if (Regex.IsMatch(phrase, startWithSkillNamePattern))
            {//呼び出し名から始まるパターン

                //呼び出し名を削除
                speechText = Regex.Replace(phrase, startWithSkillNamePattern, "");

                //つなぎ語が先頭に来るはず（つなぎ語：で、を使って、使って、に、の）
                //呼び出し名削除後に先頭がつなぎ語かチェック
                var connective = PhrasePattern.connectiveAfterSkillName.OrderByDescending(item => item.Length)
                    .FirstOrDefault(item => Regex.IsMatch(speechText, $"^{item}"));

                //つなぎ語だった場合
                //つなぎ語を削除する
                if (connective != null)
                {
                    speechText = Regex.Replace(speechText, $"^{connective}", "");
                }

                //起動フレーズが来るかも
                //来たら削除
                var invokePhrase = PhrasePattern.invokePhraseAfterSkillName.OrderByDescending(item => item.Length)
                    .FirstOrDefault(item => Regex.IsMatch(speechText, $"^{item}"));
                if (invokePhrase != null)
                {
                    speechText = Regex.Replace(speechText, $"{invokePhrase}", "");
                }

                //「聞いて」をそのままスマートスピーカへの問いかけに使うと変になるので
                speechText = Regex.Replace(speechText, askPattern, "教えて");
                speechText = $"ねえ、グーグルさん。{speechText}";

            }
            else
            {//呼び出し名から始まらないパターン

                //そもそも呼び出し名が含まれているかどうか
                if (phrase.Contains("google"))
                {
                    //呼び出し名が含まれている
                    //そのパターンは以下のとおりであることを前提とする。
                    //アクション　+　つなぎ語　+　呼び出し名　+　サポートフレーズ

                    //呼び出し名からみて、後ろがサポートフレーズ
                    //前が「アクション　+　つなぎ語」
                    var splittedPhrase = Regex.Split(phrase, "google");
                    var frontPhrase = splittedPhrase[0];
                    var rearPhrase = splittedPhrase[1];


                    //後半部がサポートフレーズから始まっているかどうか
                    var supportPhrase = PhrasePattern.supportPhraseAfterSkillName.OrderByDescending(item => item.Length)
                        .FirstOrDefault(item => Regex.IsMatch(rearPhrase, $"^{item}"));
                    if (supportPhrase == null)
                    {
                        //例外
                        speechText = "";
                        return speechText;
                    }

                    //つなぎ語を削除しない
                    var connective = PhrasePattern.connectiveAfterAction.OrderByDescending(item => item.Length)
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
                    rearPhrase = Regex.Replace(rearPhrase,askPattern,"教えて");
                    speechText = $"ねえ、グーグルさん。{frontPhrase}{rearPhrase}";
                }
                else
                {
                    //呼び出し名が含まれていない場合はそのままグーグルさんに渡す。
                    //と思ったけれど、
                    //「聞いて」をそのままGoogleへの問いかけに使うと変になるので
                    speechText = Regex.Replace(phrase, askPattern, "教えて");
                    speechText = $"ねえ、グーグルさん。{speechText}";
                }
            }



            return speechText;
        }


    }

}
