yuidoc.json is yui doc config file for auto generated javascript docs:
it should be under Nightingale.UI folder, then - 
to run it: 
	under node console go to Nightingale.UI project folder and run the command:> yuidoc -c yuidoc.json

for more details go to: http://yui.github.io/yuidoc/

option 2: without the yuidoc.json: under Nightingale.UI folder:
	>yuidoc -o "./yuidoc_out" -x "./App/test" "./App" 
	