
with open('scenario.json') as f:
    sc = f.read()

with open('icons.txt') as f:
    icons = f.readlines()


icons2 = [l.strip().split('\t') for l in icons]

for ic_data in icons2:
    name = ic_data[0]
    x = ic_data[1]
    y = ic_data[2]

    s_find = '"Name": "icons/%s-%s",' % (x, y)
    s_replace = '"Name": "icons/%s",' % name

    sc = sc.replace(s_find, s_replace)

with open('scenario.out.json', 'w') as f:
    f.write(sc)

