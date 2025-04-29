INCLUDE Globals.ink

EXTERNAL StartQuest(string id)
EXTERNAL AdvanceQuest(string id)
EXTERNAL FinishQuest(string id)

// quest names
VAR IntroductionToJaneID = "IntroductionToJane"

// quest states (quest id + "State")
VAR IntroductionToJaneQuestState = "REQUIREMENTS_NOT_MET"

INCLUDE IntroductionToJane.ink

=== SampleSwitch ===
{ IntroductionToJaneQuestState :
        - "REQUIREMENTS_NOT_MET": 
        - "CAN_START": 
        - "IN_PROGRESS":
        - "CAN_FINISH":
        - "FINISHED":
        - else: -> END
}