VAR firstSendLink = false
VAR secondSendLink = false
VAR doChores = false

->Start

=== Start ===
#Speaker: Christopher
#Emotion: Shock
Jane? Jane! 
#Speaker: Jane
#Emotion: Shock
What?! Chris? What?! What’s happen-
#Speaker: Christopher
#Emotion: Neutral
Sir’s already here! Come quick before he checks our attendance.
#Speaker: Jane
#Emotion: Shock
Wait!
* [Follow]
    -> Room103FirstClass

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
