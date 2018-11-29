using System;
using System.Collections.Generic;
using System.Text;

namespace PleaseGoogleHomeSkill_Csharp
{
    //https://developer.amazon.com/ja/docs/custom-skills/understanding-how-users-invoke-custom-skills.html　を参考に
    public static class PhrasePattern
    {
        //スキル名の後に来るサポートフレーズ

        public static readonly string[] supportPhraseAfterSkillName =
        {
            "(で|に)聞いて",
            "(で|に)開いて",
            "(で|に)確認して",
        };

        //アクションの後にくるつなぎ語

        public static readonly string[] connectiveAfterAction =
        {
            "を"
        };

        //スキル名の後に来るつなぎ語

        public static readonly string[] connectiveAfterSkillName =
        {
            "で",
            "を使って",
            "使って",
            "に",
            "の",
        };

        //アクションの後にくるサポートフレーズ

        private static string[] supportPhraseAfterAction =
        {
            "を開始して",
            "をつけて",
            "を調べて",
            "を教えて",
            "を探して",
            "を検索して",
            "をサーチ",
            "を聞いて",//教えて
            "をチェックして",
            "を教えて",
            "を確認して",
            "をチェック",
            "を見て",
            "はどう",
            "はどうですか",
            "を調べて",
        };

        //スキルネームの後にくる起動フレーズ

        public static readonly string[] invokePhraseAfterSkillName =
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
    }
}
