'''

    CHARACTER OCCURRENCE COUNTER IN PYTHON
    made by sad lad rustiniz 2020

'''

import operator

inputStr = ''
while not inputStr:
    inputStr = input("Please input a string to count characters from: ")

chDict = {}
for i in range(len(inputStr)):
    ch = inputStr[i]
    if ch not in chDict:
        chDict[ch] = 1
    else:
        chDict[ch] += 1
        
print("Occurence list: ", end = '')

# to sort characters descending by occurrence
sorted_chDict = sorted(chDict.items(), key=operator.itemgetter(1))
sorted_chDict.reverse()

printStr = ''
for ch in sorted_chDict:
    printStr += "'" + str(ch[0]) + "': " + str(ch[1]) + ', '
    
# to remove the last comma
# again, hella hacky, but eh
printStr = printStr[:-2]

print(printStr)