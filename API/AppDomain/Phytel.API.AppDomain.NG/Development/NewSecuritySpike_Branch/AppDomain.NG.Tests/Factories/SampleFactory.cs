using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Phytel.API.AppDomain.NG.Test.Factories
{
    public static class SampleFactory
    {
        public static DTO.Actions CreateAction(string id)
        {
            return new DTO.Actions
            {
                Id = id,
                ElementState = 2,
                Steps = new List<DTO.Step>
                {
                    new DTO.Step
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        ActionId = ObjectId.GenerateNewId().ToString(),
                        SourceId = ObjectId.GenerateNewId().ToString(),
                        Completed = true,
                        Order = 1,
                        Header = "asdfwerafafasdf",
                        SelectedResponseId = ObjectId.GenerateNewId().ToString(),
                        ControlType = 2,
                        SelectType = 0,
                        IncludeTime = false,
                        Question = "090s98df98dfkfasdfhaskdf",
                        Title = "Step 1324234 Title",
                        Description = "This is the description for the step 1asdfasdf324.",
                        Notes = "These are notes 234asdf23",
                        Text = "This is some text asdr32ded34",
                        Status = 2,
                        Responses = new List<DTO.Response>
                        {
                            new DTO.Response
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Order = 2,
                                Text = "Moderate",
                                StepId = ObjectId.GenerateNewId().ToString(),
                                Value = "",
                                Nominal = false,
                                Required = false,
                                NextStepId = ObjectId.GenerateNewId().ToString(),
                                Selected = false,
                                Delete = false
                            }
                        },
                        StepTypeId = 10,
                        Enabled = true,
                        StateUpdatedOn = null,
                        DateCompleted = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                        Next = ""
                    }
                }
            };
        }
        public static DTO.Module CreateModule( string actionId, string step1Id, string step2Id, string selectedRespId)
        {
            var module =
                new DTO.Module
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Actions = new List<DTO.Actions>
                    {
                        new DTO.Actions
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            ElementState = 2,
                            Next = actionId,
                            Previous = actionId,
                            SpawnElement = new List<DTO.SpawnElement>
                            {
                                new DTO.SpawnElement
                                {
                                    ElementId = actionId
                                }
                            },
                            Steps = new List<DTO.Step>
                            {
                                new DTO.Step
                                {
                                    Id = step1Id,
                                    ActionId = actionId,
                                    SourceId = ObjectId.GenerateNewId().ToString(),
                                    Completed = true,
                                    Order = 1,
                                    Header = "Step 1 header",
                                    SelectedResponseId = selectedRespId,
                                    ControlType = 2,
                                    SelectType = 0,
                                    IncludeTime = false,
                                    Question = "Step 1 Question",
                                    Title = "Step 1 Title",
                                    Description = "This is the description for the step 1.",
                                    Notes = "These are notes",
                                    Text = "This is some text.",
                                    Status = 2,
                                    Responses = new List<DTO.Response>
                                    {
                                        new DTO.Response
                                        {
                                            Id = selectedRespId,
                                            Order = 2,
                                            Text = "Moderate",
                                            StepId = step1Id,
                                            Value = "",
                                            Nominal = false,
                                            Required = false,
                                            NextStepId = step2Id,
                                            Selected = false,
                                            Delete = false,
                                            SpawnElement = new List<DTO.SpawnElement>
                                            {
                                                new DTO.SpawnElement
                                                {
                                                    ElementId = actionId
                                                }
                                            }
                                        }
                                    },
                                    StepTypeId = 10,
                                    Enabled = true,
                                    StateUpdatedOn = null,
                                    DateCompleted = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                                    Next = "",
                                    SpawnElement = new List<DTO.SpawnElement>
                                    {
                                        new DTO.SpawnElement
                                        {
                                            ElementId = actionId
                                        }
                                    }
                                }
                            }
                        },
                        new DTO.Actions
                        {
                            Id = actionId,
                            ElementState = 2,
                            Steps = new List<DTO.Step>
                            {
                                new DTO.Step
                                {
                                    Id = step1Id,
                                    ActionId = actionId,
                                    SourceId = ObjectId.GenerateNewId().ToString(),
                                    Completed = true,
                                    Order = 1,
                                    Header = "Step 1 header",
                                    SelectedResponseId = selectedRespId,
                                    ControlType = 2,
                                    SelectType = 0,
                                    IncludeTime = false,
                                    Question = "Step 1 Question",
                                    Title = "Step 1 Title",
                                    Description = "This is the description for the step 1.",
                                    Notes = "These are notes",
                                    Text = "This is some text.",
                                    Status = 2,
                                    Responses = new List<DTO.Response>
                                    {
                                        new DTO.Response
                                        {
                                            Id = selectedRespId,
                                            Order = 2,
                                            Text = "Moderate",
                                            StepId = step1Id,
                                            Value = "",
                                            Nominal = false,
                                            Required = false,
                                            NextStepId = step2Id,
                                            Selected = false,
                                            Delete = false
                                        }
                                    },
                                    StepTypeId = 10,
                                    Enabled = true,
                                    StateUpdatedOn = null,
                                    DateCompleted = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                                    Next = ""
                                }
                            }
                        }
                    }
                };

            return module;
        }

        internal static DTO.Actions CreateCloneAction(string actionId, string step1Id, string step2Id, string selectedRespId)
        {
            return new DTO.Actions
            {
                Id = actionId,
                ElementState = 2,
                Steps = new List<DTO.Step>
                {
                    new DTO.Step
                    {
                        Id = step1Id,
                        ActionId = actionId,
                        SourceId = ObjectId.GenerateNewId().ToString(),
                        Completed = true,
                        Order = 1,
                        Header = "Step 1 header",
                        SelectedResponseId = selectedRespId,
                        ControlType = 2,
                        SelectType = 0,
                        IncludeTime = false,
                        Question = "Step 1 Question",
                        Title = "Step 1 Title",
                        Description = "This is the description for the step 1.",
                        Notes = "These are notes",
                        Text = "This is some text.",
                        Status = 2,
                        Responses = new List<DTO.Response>
                        {
                            new DTO.Response
                            {
                                Id = selectedRespId,
                                Order = 1,
                                Text = "Testing",
                                StepId = step1Id,
                                Value = "",
                                Nominal = false,
                                Required = false,
                                NextStepId = step2Id,
                                Selected = false,
                                Delete = false
                            },
                            new DTO.Response
                            {
                                Id = selectedRespId,
                                Order = 2,
                                Text = "Moderate",
                                StepId = step1Id,
                                Value = "",
                                Nominal = false,
                                Required = false,
                                NextStepId = step2Id,
                                Selected = false,
                                Delete = false
                            }
                        },
                        StepTypeId = 10,
                        Enabled = true,
                        StateUpdatedOn = null,
                        DateCompleted = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                        Next = ""
                    },
                    new DTO.Step
                    {
                        Id = step2Id,
                        ActionId = actionId,
                        SourceId = ObjectId.GenerateNewId().ToString(),
                        Completed = true,
                        Order = 2,
                        Header = "Step 2 header",
                        SelectedResponseId = "",
                        ControlType = 2,
                        SelectType = 0,
                        IncludeTime = false,
                        Question = "Step 2 Question",
                        Title = "Step 2 Title",
                        Description = "This is the description for the step 1.",
                        Notes = "These are notes extra",
                        Text = "This is some text.",
                        Status = 2,
                        Responses = new List<DTO.Response>
                        {
                            new DTO.Response
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                Order = 2,
                                Text = "Moderate",
                                StepId = step2Id,
                                Value = "",
                                Nominal = false,
                                Required = false,
                                NextStepId = null,
                                Selected = false,
                                Delete = false,
                                SpawnElement = new List<DTO.SpawnElement>
                                {
                                    new DTO.SpawnElement
                                    {
                                        ElementId = "1234",
                                        ElementType = 3,
                                        Tag = "77387092394029347"
                                    }
                                }
                            }
                        },
                        StepTypeId = 10,
                        Enabled = true,
                        StateUpdatedOn = null,
                        DateCompleted = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                        Next = ""
                    }
                }
            };
        }
    }
}
