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

=== LaiExtraDialogue ===
{ JaneIsSeated == false:
    It's really a matter of time until someone fails the subject.  #Speaker: Lai #Layout: Left
}
-> DONE

=== NoelleExtraDialogue ===
{ JaneIsSeated == false:
    I honestly don't know if I can pass this subject anymore.  #Speaker: Noelle #Layout: Left
    My guardian angel has been very weak lately.  #Speaker: Noelle #Layout: Left
}
-> DONE

=== NoellesExtraDialogue ===
{ JaneIsSeated == false:
    I hope things turn out for the better. This really isn't a good sign.  #Speaker: Noelle #Layout: Left
}
-> DONE

=== JaneSeated ===
Did something happen? It seems that something’s wrong in the air. #Speaker: Jane #Layout: Left
Apparently, sir isn’t really in the mood. He wasn’t too happy with the performance of our latest laboratory. #Speaker: Pipes #Layout: Right


* [Why do you think that is?]
    I don't know but I feel like we messed up big time. #Speaker: Pipes #Layout: Right
* [Did we mess up again?]
    Guessing from the overall mood, I think so. 

- Yeah, and he’s really pissed. #Speaker: Christopher #Layout: Right
~ JaneIsSeated = true
-> DONE


=== HeinrichEnter ===
~HeinrichIntroduced = true
Good morning, class. #Speaker: Heinrich #Layout: Left
Good morning... #Speaker: Jane #Layout: Left
Good morning, sir! #Speaker: Lai #Layout: Right
Good morning, sir. #Speaker: Noelle #Layout: Right
->DONE

=== HeinrichDisappoint ===
I have recently checked your latest laboratory exercise, and honestly I haven’t been the most disappointed in my life. #Speaker: Heinrich #Layout: Left
I’ve designed it to be difficult to understand, but easy to solve. #Speaker: Heinrich #Layout: Left
I’ve added all those additional information to confuse you, but only a few managed to find the right answer. #Speaker: Heinrich #Layout: Left
You’re already in your third year, a year closer to graduation - that is if you can do so on time. #Speaker: Heinrich #Layout: Left
Please be reminded that your occupation and the world does not spoonfeed you the answers.#Speaker: Heinrich #Layout: Left
With the evergrowing busy society, thousands of irrelevant information bombard you. #Speaker: Heinrich #Layout: Left
Navigating even the simplest answers are made challenging. How can you really tell that you’re ready for the industry if you fail to recognise a simple task? #Speaker: Heinrich #Layout: Left
-> DONE

=== PipesBlurtComment ===
He’s definitely not on a good mood. #Speaker: Pipes #Layout: Right
-> DONE

=== HeinrichSideEye ===
... #Speaker: Heinrich #Layout: Left
I expect you here to be the cream of the crop once you finish this subject, and I will not allow any student to pass this subject completely unprepared. #Speaker: Heinrich #Layout: Left
Do I make myself clear? #Speaker: Heinrich #Layout: Left
Yes, sir. #Speaker: Lai #Layout: Right
Yes, sir. #Speaker: Noelle #Layout: Right
... #Speaker: Jane #Layout: Left
-> DONE

=== HeinrichAnger ===
Good. Let me remind everyone that our finals week is approaching. #Speaker: Heinrich #Layout: Left
The latest laboratory exercise has affected most of you, so this term exam will dictate whether you pass or fail this course. #Speaker: Heinrich #Layout: Left
The passing rate will remain the same, and there will be no removal exams. #Speaker: Heinrich #Layout: Left 
To prove that a student is worthy of passing my subject in a single test despite having the entire semester to do so is utterly ridiculous. #Speaker: Heinrich #Layout: Left
-> DONE

=== ChristopherSideComment ==
I’ve never seen him this angry. Was our performance really that bad? #Speaker: Christopher #Layout: Right
-> DONE 

=== JaneCommentResponse
I think I messed the entire thing up. #Speaker: Jane #Layout: Left
I think I did too. Not to mention that its weight is higher than the other laboratories. We’re all doomed. #Speaker: Christopher #Layout: Right
-> DONE

=== HeinrichRemarks ===
This is your final chance to redeem yourselves. Please don’t disappoint me any further.#Speaker: Heinrich #Layout: Left
Please wait a while.#Speaker: Heinrich #Layout: Left
-> DONE

=== LaiQuestion ===
But, sir. How about the discussion? I think we still have some chapters left in our module that are included in the coverage. #Speaker: Lai #Layout: Right
Do we have to do independent study or d- #Speaker: Lai #Layout: Right
A lecture session will be conducted later this 4 in the afternoon. In the meantime, use the remaining time to reflect on your recent performance. #Speaker: Heinrich #Layout: Left
Don’t forget to submit your attendance slips at the table in front. Please form in line, as well. I will be returning your Laboratory Exercises shortly. #Speaker: Heinrich #Layout: Left
-> DONE

=== ChristopherBeforeGoingInLine ===
{ HeinrichIntroduced == true:
    Guys, I think I’m gonna cry. I really feel like I failed the exercise. #Speaker: Jane #Layout: Left
    Jane, trust me. There’s only one way to find out~ #Speaker: Pipes #Layout: Right
    Pipes, the girl is about to cry! Don’t make it worse. #Speaker: Christopher #Layout: Right
    I’M NOT! I’m just trying to lighten the mood, that’s all. #Speaker: Pipes #Layout: Right
    ... #Speaker: Jane #Layout: Left
    Hey, hey. I’m sorry. What matters is that you tried your best, and if things comes to worse, which I hope things wont, we still have the final term exam. #Speaker: Pipes #Layout: Right
    Yeah, we can still pass this horrid subject. Let’s fall in line and meet our inevitable fate. I wanna get out of this room as quickly as possible. #Speaker: Christopher #Layout: Right
    I agree. Let's get outta here! #Speaker: Pipes #Layout: Right
    ~ TalkedToChristopherBeforeFallingInLine = true
}
-> DONE

=== RetrievedPaper ===
{ TalkedToChristopherBeforeFallingInLine == true && ClaimedPaper == false:
    ~ ClaimedPaper = true
    ... #Speaker: Heinrich #Layout: Right
    Jane... #Speaker: Heinrich #Layout: Right
    Yes, sir? #Speaker: Jane #Layout: Left
    Hmmm... #Speaker: Heinrich #Layout: Right
    Disappointed, but not surprised... Claim your paper. #Speaker: Heinrich #Layout: Right
    Yes, sir. #Speaker: Jane #Layout: Left
}
-> DONE