INCLUDE Globals.ink

EXTERNAL StartQuest(string id)
EXTERNAL AdvanceQuest(string id)
EXTERNAL FinishQuest(string id)

=== Start ===
Jane? Jane! #Speaker: Christopher #Layout: Right
What?! Chris? What?! What’s happen- #Speaker: Jane #Layout: Left
Sir’s already here! Come quick before he checks our attendance. #Speaker: Christopher #Layout: Right
Wait! #Speaker: Jane #Layout: Left
* [Follow]
    PAGDALI! #Speaker: Christopher #Layout: Right
    ~ StartQuest("IntroductionToJane")
* [Continue Sleeping]
    Bahala ka araa, uy! #Speaker: Christopher #Layout: Right

- Ok #Speaker: Jane #Layout: Left
-> DONE
