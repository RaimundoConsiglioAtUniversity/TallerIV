# Editing Regex
List of regex tha can help with editing my works.

## Filter Words
\b(Able|Assume|Assumes|Assumed|Believe|Believes|Believed|Can|Cannot|Can't|Could|Couldn't|Decide|Decides|Decided|Experience|Experiences|Experienced|Feel|Feels|Felt|Hear|Hears|Heard|Know|Knows|Knew|Look|Looks|Looked|Note|Notes|Noted|Notice|Notices|Noticed|Realize|Realizes|Realized|Remember|Remembers|Remembered|See|Sees|Saw|Seem|Seems|Seemed|Smell|Smells|Smelled|Sound|Sounds|Sounded|Taste|Tastes|Tasted|Think|Thinks|Thought|Touch|Touches|Touched|Watch|Watches|Watched|Wonder|Wonders|Wondered)\b

## Intensifiers
\b(Absolutely|Completely|Totally|Just|Really|Very|Even|Quite|Rather)\b



## Filler
\b(all of the|as to whether|probably|start|begin|began|begun|somewhat|somehow|at all times|commonly|Certainly|Definitely|\w then \w|For all intents and purposes|for the purpose of|Has the ability to|The fact that|In terms of|In the event that|In the process of|in order to|it is important to note|It is possible that|needless to say)\b

## That
\b(that|then)\b

## Adverbs
\b\w*ly\b

## Weak Verbs
\b(take|takes|took|taking|make|makes|made|making|give|gives|gave|giving|get|got|getting) (me |you |us |them |her |him |it )(the |a |an |)\w*\b


https://authorhelp.uk/editing-use-regular-expressions-find-common-errors/

## Repeated Words
\b(\w+)\b \b\1\b

## Periods & Commas Outside Quotes
"[.,]

## "A" instead of "An"
\Wa [aeiou]

## "An" instead of "A"
\Wan [^aeiou]

## Missing Oxford Commas
\w+, \w+ and 

## Missing Caps // Match Case
[^\.](\. |\n|\r)"?—?\*{0,3}[a-z]

## Extra Caps in Sentence
\b(?!\. |\n|\r)"?—?\*{0,3} [A-Z]

## Uncaptilized Is
\bi\b

## Missing Parenthesis
\([^)]*$

## Wrong Paragraph Punctuation
^[^#\[=\*\[\-)<].*(?<!(!|\?|‽|\.|:|"|!\*|!"|!\)|!\]|!—|!~|!>|\?\*|\?"|\?\)|\?\]|\?—|\?~|\?>|‽\*|‽"|‽\)|‽\]|‽—|‽~|‽>|\.\*|\."|\.\)|\.\]|\.—|\.~|\.>|:\*|:"|:\)|:\]|:—|—|:~|:>|>))$

## Uneven Quotes
^(?:"[^"]*"|[^"])*"(?:"[^"]*"|[^"])*$

^((# )|(\[?([a-z A-Z,\.]*,? )*(,? and [a-z A-Z,\.]*)?(Sir )?(Young )?(Past )?(Future )?(Baby )?(Filly )?(Colt )?(Younger )?(Old )?(Princess )?(Prince )?((Pinkie)|(Pinkamena .*))( Pie)?([a-z A-Z,\.]*)(\](.*\n(?!^.*: ))*|(: )))).*

^((# )|(\[?([a-z A-Z,\.]*,? )*(,? and [a-z A-Z,\.]*)?(Sir )?(Young )?(Past )?(Future )?(Baby )?(Filly )?(Colt )?(Younger )?(Old )?(Princess )?(Prince )?((Twilight)|(Sci.*Twi))( Sparkle)?([a-z A-Z,\.]*)(\](.*\n(?!^.*: ))*|(: )))).*

^((# )|(\[?([a-z A-Z,\.]*,? )*(,? and [a-z A-Z,\.]*)?(Sir )?(Young )?(Past )?(Future )?(Baby )?(Filly )?(Colt )?(Younger )?(Old )?(Princess )?(Prince )?Spike( The Dragon)?([a-z A-Z,\.]*)(\](.*\n(?!^.*: ))*|(: )))).*