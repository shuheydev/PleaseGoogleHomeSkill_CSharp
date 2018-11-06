using System;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System.Text.RegularExpressions;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PleaseGoogleHomeSkill_Csharp
{
    public class Function
    {
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
                        skillResponse = LaunchRequestHandler(launchRequest);
                        break;
                    case IntentRequest intentRequest:
                        switch (intentRequest.Intent.Name)
                        {
                            case "DelegateGoogleHomeIntent":
                                skillResponse = DelegateGoogleHomeIntentHandler(intentRequest);
                                break;
                            case "AMAZON.HelpIntent":
                                skillResponse = HelpIntentHandler(intentRequest);
                                break;
                            case "AMAZON.CancelIntent":
                                skillResponse = CancelAndStopIntentHandler(intentRequest);
                                break;
                            case "AMAZON.StopIntent":
                                skillResponse = CancelAndStopIntentHandler(intentRequest);
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

        private SkillResponse LaunchRequestHandler(LaunchRequest launchRequest)
        {
            var speechText = "グーグルさんに何を訊きますか？";

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
            skillResponse.Response.Card = new SimpleCard
            {
                Title = "グーグルさん",
                Content = speechText
            };

            return skillResponse;
        }

        private SkillResponse DelegateGoogleHomeIntentHandler(IntentRequest intentRequest)
        {
            var phrase = intentRequest.Intent.Slots["phrase"].Value;

            var speechText = "";

            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };


            if (string.IsNullOrWhiteSpace(phrase))
            {
                speechText = "すみません。聞き取れませんでした。もう一度言ってください。";

                skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
                {
                    Text = phrase
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






            speechText = Phrase.ComposeAskGoogleHomeText(phrase);

            skillResponse.Response.OutputSpeech = new PlainTextOutputSpeech
            {
                Text = speechText
            };
            skillResponse.Response.Card = new SimpleCard
            {
                Title = "グーグルさん",
                Content = speechText
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }

        private SkillResponse HelpIntentHandler(IntentRequest intentRequest)
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


        private SkillResponse CancelAndStopIntentHandler(IntentRequest intentRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };
            skillResponse.Response.ShouldEndSession = true;

            return skillResponse;
        }


        private SkillResponse SessionEndedRequestHandler(SessionEndedRequest sessionEndedRequest)
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
            var speechText = "すみません。聞き取れませんでした。に";

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

            return skillResponse;
        }

        #endregion
    }
}
