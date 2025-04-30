=== IntroductionToJaneStart ===
Jane? Jane! #Speaker: Christopher #Layout: Right
What?! Chris? What?! What’s happen- #Speaker: Jane #Layout: Left
Sir’s already here! Come quick before he checks our attendance. #Speaker: Christopher #Layout: Right
Wait! #Speaker: Jane #Layout: Left
~StartQuest(IntroductionToJaneID)
-> DONE

=== CallingJaneInRoom103 ===
Pssst. Jane over here! #Speaker: Christopher #Layout: Right
-> DONE

=== PipesExtraDialogue ===
{ JaneIsSeated == false: 
    Jane, be seated quick. It's gonna be a rough day today. #Speaker: Pipes #Layout: Right
}
-> DONE


=== JaneSeated ===
~ JaneIsSeated = true
Did something happen? It seems that something’s wrong in the air. #Speaker: Jane #Layout: Left
Apparently, sir isn’t really in the mood. He wasn’t too happy with the performance of our latest laboratory. #Speaker: Pipes #Layout: Right
Yeah, and he’s really pissed. #Speaker: Christopher #Layout: Right
-> DONE


=== HeinrichEnter ===
Good morning, class. #Speaker: Heinrich #Layout: Left
Good morning... #Speaker: Jane #Layout: Left
Good morning, sir! #Speaker: Lai #Layout: Right
Good morning, sir. #Speaker: Noelle #Layout: Right
->DONE

=== HeinrichDisappoint ===
I have recently checked your latest laboratory exercise, and honestly I haven’t been the most disappointed in my life. #Speaker: Heinrich #Layout: Left
I’ve designed it to be difficult to understand, but easy to solve. #Speaker: Heinrich #Layout: Left
I’ve added all those additional information to confuse you, but only a few managed to find the right answer. #Speaker: Heinrich #Layout: Left
-> DONE


=== HeinrichAnger ===
You’re already in your third year, a year closer to graduation - that is if you can do so on time. #Speaker: Heinrich #Layout: Left
Please be reminded that your occupation and the world does not spoonfeed you the answers, and with the evergrowing busy society, thousands of irrelevant information bombard you. #Speaker: Heinrich #Layout: Left
Navigating even the simplest answers are made challenging. How can you really tell that you’re ready for the industry if you fail to recognise a simple task? #Speaker: Heinrich #Layout: Left
-> DONE