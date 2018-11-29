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
        private readonly string _skillTitleForCard = "���肢�O�[�O������";

        private readonly string _phraseKeyName = "phrase";


        //�e�X�g
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
         
                //1.LaunchRequest��IntentRequest��
                //�^�X�C�b�`���g�������@
                switch (skillRequest.Request)
                {
                    case LaunchRequest launchRequest://�^��LaunchRequest�������ꍇ�ɁARequest��LaunchRequest�ɃL���X�g���āA�ϐ�launchRequest�ɓ���Ă����B�֗�
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


        #region �e�C���e���g�A���N�G�X�g�ɑΉ����鏈����S�����郁�\�b�h����

        private SkillResponse LaunchRequestHandler(SkillRequest skillRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody()
            };


            var speechText = $"�O�[�O������ɉ���u���܂����H";

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


            //"phrase"�X���b�g�ɉ��������Ă��Ȃ������ꍇ�͂����ł����܂�
            if (string.IsNullOrWhiteSpace(phrase))
            {
                speechText = "���݂܂���B�������܂���ł����B������x�����Ă��������B";

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
            var speechText = "�������̂ƂȂ�ɃO�[�O�����񂪂���̂Ȃ�A������O�[�O������ɐu���Ă݂܂��B"
                + "�Ⴆ�΁A�O�[�O��������J���Ė����̓V�C�������āA�Ɛu���Ă݂Ă��������B";

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
                Title = "�O�[�O������",
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
            var speechText = "���݂܂���B�������܂���ł����B";

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
