# Hair Pandemic - Android game

Play demo at [Google Play](https://play.google.com/store/apps/details?id=com.Evorsion.HairPandemic)

# Roadmap

| Status | Goal               | Labels |
| :----: | :----------------- | :----- |
|   ✔    | Movement           | `done` |
|   ✔    | Fighting and stats | `done` |
|   ✔    | Merge mechanic     | `done` |
|   ✔    | Level switching    | `done` |
|   ✔    | Menu               | `done` |
|   ✔    | Level design       | `done` |
|   ✔    | Gameplay tuning    | `done` |
|   ✔    | GUI                | `done` |
|   ✔    | Story              | `done` |



# Specifikace

**Téma práce:** 

Mobilní hra na systém Android

**Vedoucí:**

Mgr. Lukáš Kolek, Ph.D.

 

## Specifikace software

Cílem tohoto ročníkového projektu bude vytvořit hru s ovládáním a hratelností přizpůsobenými mobilní platformě Android. Základním cílem hry bude porazit a zkonvertovat co nejvíce protivníků do hráčova davu. Hráč toho může dosáhnout taktickým vybíráním potyček se skupinami protivníků.

Je nutné klást velký důraz na UX, tak aby hráč nebyl z ovládání, hratelnosti a orientace frustrovaný.

### Herní mechaniky:

- Autentický pohyb skupiny
- Konverze jednotek mezi skupinami
- Kamera sledující zadanou skupinu
- Slučování jednotek do silnějších
- Boj mezi nepřátelskými jednotkami
- Interaktivní prvky prostředí
- Minimapa pro lepší orientaci v úrovních

### Detailní popis mechanik



Hráč ovládá svou skupinu pomocí joysticku v dolní části obrazovky.

Skupina se vydává primárně ve směru joysticku, ale pohyb jednotlivců je doplněn o náhodné chování a interakce se skupinou.

Interakce se skupinou sestávají ze tří indikátorů sečtených do jednoho vektoru.

Indikátor soudržnosti posílá jednotlivce směrem k ostatním jednotkám jeho skupiny.

Indikátor zarovnání posílá jednotlivce stejným směrem jako míří ostatní jednotky jeho skupiny.

Indikátor separace posílá jednotlivce směrem od ostatních jednotek jeho skupiny.

Nepřátelské jednotky jsou uspořádané do skupin. Každá skupina má vymezené území na kterém se pohybuje náhodnými procházkami.

Nepřátelská jednotka se vždy pohybuje k cílovému bodu procházky. Skupina dosáhne cíle pouze pokud průměrná pozice skupiny je v blízkosti cíle.

Pokud se přiblíží hráčská jednotka nepřátelské (a naopak) na dosah jejich zbraně, začne útočit ve specifikovaných intervalech konstantním poškozením.

Pokud dosáhne nepřátelská jednotka úrovně zdraví rovné nule, promění se v hráčskou jednotku stejné úrovně s plným zdravím.

Pokud hráčská jednotka dosáhne úrovně zdraví rovné nule, promění se v nepohyblivou neutrální jednotku.

Po konci každé úrovně nastane fáze slučování jednotek. Hráč přetažením jednotky na jinou jednotku stejné úrovně sloučí tyto jednotky do jedné o úroveň vyšší jednotky.

Jednotka druhé úrovně je asi třikrát silnější než jednotka první úrovně. Je tomu tak aby byl hráč motivovaný do slučování jednotek na silnější.

Po celou dobu hry je kamera zaměřená tak aby byla vidět každá hráčská jednotka, ale zároveň je oddálení kamery co nejmenší.

V rohu obrazovky se vyskytuje minimapa, která zobrazuje pozici všech jednotek v úrovni.

Některé úrovně jsou opatřené pastmi, ve kterých se může libovolná jednotka zranit.

 

## Specifikace výtvarného ztvárnění

Hráč reprezentuje rebelující skupinu lidí co se rozhodli obarvit si vlasy na zelenou. Jejich cílem je obarvit každého na zelenou. Jejich prostředkem jsou spreje se zelenou barvou. Opačná strana se barvení brání a uchylují se k oholení zelených vlasů jejich nepřátel, což je paralyzuje (neutrální jednotka).

Hráč začíná doma a postupně se dostane do blízkého sousedství, vesnice a končí ve městě. Úrovně je potřeba ohraničit (graficky i herně), aby se hráč nepokoušel odejít z oblasti. Jednotlivé úrovně jednotek jsou (i) výrostek; (ii) chlap; (iii) kadeřnice; (iv) bodybuilder.

Styl grafiky bude pixel art.



## Specifikace dokumentace

Dokumentace popíše fungování složitějších mechanik pro lepší pochopení provedených rozhodnutí. Důraz bude kladen na zdokumentování postupu k rozšíření. Hra sice bude obsahovat pouze 4 úrovně a 4 jednotky, ale mělo by být možné do budoucna přidat další pouze se znalostí obsažených v dokumentaci.

 