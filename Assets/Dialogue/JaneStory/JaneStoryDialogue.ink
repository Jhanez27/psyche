VAR firstSendLink = false
VAR secondSendLink = false
VAR doChores = false

->Start

=== Start ===
Jane? Jane! #Speaker: Christopher #Emotion: Shock #Portrait: Right
What?! Chris? What?! What’s happen- #Speaker: Jane #Emotion: Shock #Portrait: Left
Sir’s already here! Come quick before he checks our attendance. #Speaker: Christopher #Emotion: Neutral #Portrait: Right
Wait! #Speaker: Jane #Emotion: Shock #Portrait: Left
* [Follow]
    PAGDALI! #Speaker: Christopher #Emotion: Neutral #Portrait: Right
    -> Room103FirstClass
* [Continue Sleeping]
    Bahala ka araa, uy! #Speaker: Christopher #Emotion: Neutral #Portrait: Right
    ->Room103FirstClass

=== Room103FirstClass ===
#Speaker: Christopher
#Emotion: Neutral
Pssttt, Jane. Over here!
* [Go to Christopher]
    -> JaneWalkingRoom103FirstClass

=== JaneWalkingRoom103FirstClass ===
#Speaker: Jane
#Emotion: Neutral
Did something happen? It seems that something’s wrong in the air.
#Speaker: Pipes
#Emotion: Sad
Apparently, sir isn’t really in the mood. He wasn’t too happy with the performance of our latest laboratory.
#Speaker: Christopher
#Emotion: Sad
Yeah, and he’s really pissed. 
* [Sit and wait]
    -> HeinrichSpeaking
    
=== HeinrichSpeaking ===

-> DONE
