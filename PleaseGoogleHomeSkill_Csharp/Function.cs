using System;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System.Text.RegularExpressions;
using AlexaPersistentAttributesManager;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PleaseGoogleHomeSkill_Csharp
{
    public partial class Function
    {
        private readonly string _skillTitleForCard = "お願いグーグルさん";

        private readonly string _phraseKeyName = "phrase";


        //テスト
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="skillRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest skillRequest, ILambdaContext context)
        {
            SkillResponse skillResponse = null;

            try
            {
         
                //1.LaunchRequestかIntentRequestか
                //型スイッチを使った方法
                switch (skillRequest.Request)
                {
                    case LaunchRequest launchRequest://型がLaunchRequestだった場合に、RequestをLaunchRequestにキャストして、変数launchRequestに入れてくれる。便利
                        skillResponse = LaunchRequestHandler(skillRequest);
                        break;
                    case IntentRequest intentRequest:
                        switch (intentRequest.Intent.Name)
                        {
                            case "DelegateGoogleHomeIntent":
                                skillResponse = DelegateSmartSpeakerIntentHandler(skillRequest);
                                break;
                            case "AMAZON.HelpIntent":
                                skillResponse = HelpIntentHandler(skillRequest);
                                break;
                            case "AMAZON.CancelIntent":
                                skillResponse = CancelAndStopIntentHandler(skillRequest);
                                break;
                            case "AMAZON.StopIntent":
                                skillResponse = CancelAndStopIntentHandler(skillRequest);
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }
            }
            catch
            {
                skillResponse = ErrorHandler(skillRequest);
            }

            return skillResponse;
        }


        #region 各インテント、リクエストに対応する処理を担当するメソッドたち

        private SkillResponse LaunchRequestHandler(SkillRequest skillRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };


            var speechText = $"グーグルさんに何を訊きますか？";

            skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
            {
                Text = speechText
            };
            skillResponse.Response.Reprompt = new Reprompt
            {
                OutputSpeech = new PlainTextOutputSpeech
                {
                    Text = speechText
                }
            };
            skillResponse.Response.Card = new SimpleCard
            {
                Title =_skillTitleForCard,
                Content = speechText
            };

            return skillResponse;
        }

        private SkillResponse DelegateSmartSpeakerIntentHandler(SkillRequest skillRequest)
        {
            var intentRequest = skillRequest.Request as IntentRequest;
            var phrase = intentRequest.Intent.Slots[_phraseKeyName].Value;

            var speechText = "";

            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };


            //"phrase"スロットに何も入っていなかった場合はここでおしまい
            if (string.IsNullOrWhiteSpace(phrase))
            {
                speechText = "すみません。聞き取れませんでした。もう一度言ってください。";

                skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
                {
                    Text = speechText
                };
                skillResponse.Response.Reprompt = new Reprompt
                {
                    OutputSpeech = new PlainTextOutputSpeech
                    {
                        Text = speechText
                    }
                };

                return skillResponse;
            }


            speechText = Phrase.ComposeAskSmartSpeakerText(phrase);

            skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
            {
                Text = speechText
            };
            skillResponse.Response.Card = new SimpleCard
            {
                Title =_skillTitleForCard,
                Content = speechText
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }

        private SkillResponse HelpIntentHandler(SkillRequest skillRequest)
        {
            var speechText = "もし私のとなりにグーグルさんがいるのなら、私からグーグルさんに訊いてみます。"
                + "例えば、グーグルさんを開いて明日の天気を教えて、と訊いてみてください。";

            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };

            skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
            {
                Text = speechText
            };
            skillResponse.Response.Card = new SimpleCard
            {
                Title = "グーグルさん",
                Content = speechText
            };

            return skillResponse;
        }


        private SkillResponse CancelAndStopIntentHandler(SkillRequest skillRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }


        private SkillResponse SessionEndedRequestHandler(SkillRequest skillRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }


        private SkillResponse ErrorHandler(SkillRequest skillRequest)
        {
            var speechText = "すみません。聞き取れませんでした。";

            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };

            skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
            {
                Text = speechText
            };
            skillResponse.Response.Reprompt = new Reprompt
            {
                OutputSpeech = new PlainTextOutputSpeech
                {
                    Text = speechText
                }
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }

        #endregion
    }
}
