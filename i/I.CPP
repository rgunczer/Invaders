#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <graphics.h>
#include <dos.h>


#define ESCAPE 27
#define FIRE 32
#define BAL (75+256)
#define JOBB (77+256)
#define SPEED 7
#define ROCKCOLOR 7
#define P_XPOS 243;
#define P_YPOS 440;

/* IDOZITOK */
#define MOVE 60
#define WORK 30
#define SHOOT 5


typedef struct bullet {
                       struct bullet* nextp;
                       int sx, sy, who;
                       int sactive;
                      }
BULLET;

typedef struct listafej {
                         BULLET* startp;
                         BULLET* endp;
                         short ures;
                        }
LISTAFEJ;

typedef struct enemy {
                      int ex, ey;
                      int eactive;
                     }
ENEMY;



/* Csillagok koordinatai */
int stx[205]; /* <-- Stars x position */
int sty[205]; /* <-- Stars y position */

ENEMY cel[54]; /* <-- 54 elemu tombben az ellenseg adatai */

unsigned int xcp, ycp, destroyed, war, lives, hits, leh, right, caunter;

void *player_ship;
void *enemy_ship;
void *live_icon;

LISTAFEJ lista;
BULLET* elemp;

void Read2(void);
void Read3(void);
void Game(void);
void Rocks(void);
void Side(void);
void Stars(void);
void Init_Enemy(void);
void Graphinit(void);
void Redraw(void);
void Moveright(void);
void Moveleft(void);
void Stepdown(void);


int Ujgetch(void);
int Game_Over(void);


void InitList(LISTAFEJ* fej);
void Insert(LISTAFEJ* lista, BULLET* elem);
void InitElem(BULLET* elem);
void EinitElem(BULLET* elem, int a);
void Bullet_Server(LISTAFEJ* lista);
void Bullet_Killer(LISTAFEJ* lista);
void Score(int hts);
void Read(int where);


// --------------- Belepes a programba -------------

int main()
{
 Graphinit();
/*
 setcolor(11);
 settextstyle(0,0,7);
 outtextxy(100,200,"Invaders");
 */
 Read2();


 outtextxy(240,280,"Press any key...");

 getch();

 cleardevice();
 Game();

// Read(1);

 closegraph();
 return 0;
} /* End Of Main */


void Game(void)
{
 int time, movetime, worktime, felso, falso, dcolor, hcolor, leh1, a1, size, flip;
 char ch;

 InitList(&lista);

 /*  ACTORS */

 // player();
 /*
  setcolor(15);
  setfillstyle(1,1);
    //190,100,235,120
  bar(190,100,225,132);



  line(210,101,210,108);
  line(212,101,212,108);
  line(211,106,211,108);
   */
Read(1);





// getch();


  size = imagesize(190,100,235,132);
  player_ship = malloc(size);
  getimage(190,100,235,132, player_ship);

  putimage(190, 100 ,player_ship,XOR_PUT);


 //enemy();

// setcolor(7);
/*
 setfillstyle(1,3);
 bar(190,100,235,120);
 getch();
  */
Read(2);

// getch();

 size = imagesize(190,100,235,123);

 enemy_ship = malloc(size);
 getimage(190, 100, 235, 123, enemy_ship);
// getch();
 putimage(190, 100, enemy_ship, XOR_PUT);


Read3();

/* //playerliveicon();
 setcolor(15);
 setfillstyle(1,15);
 bar(550,300,565,315);
// getch();
*/
 size = imagesize(550, 300, 571, 321);
 live_icon = malloc(size);
 getimage(550, 300, 571, 321, live_icon);
 putimage(550, 300, live_icon, XOR_PUT);






 Stars();
 Rocks();

 xcp=P_XPOS;
 ycp=P_YPOS;
 putimage(xcp, ycp, player_ship, COPY_PUT);



 Init_Enemy(); /* < -- Initialize Invaders */


 /* Valtozok Inicializalasa */

 lives=3;
 caunter=0;

 movetime=MOVE;
 worktime = WORK;
 time = SHOOT;

 right =1;
 war=1;
 leh1=0;
 dcolor=9;
 hcolor=0;
 flip=1;
 destroyed=0;
 hits=0;

 Side(); /* <-- Kepernyo szelenek kirajzolasa */
 // Score(hits); nincs meg kinyomtatva!!!

/* Draw Lives */
 putimage(601, 415, live_icon, XOR_PUT);

 putimage(579, 415, live_icon, XOR_PUT);

 putimage(557, 415, live_icon, XOR_PUT);




/* ----------- */
/* The Reactor */
/* ----------- */


for (caunter=0;caunter!=900; caunter++)
{
 delay(SPEED);

 if (caunter >= 801)    /* <-- Reactor ujrainditasa */
  {
   Redraw();
   caunter=0;
   time=SHOOT;
   movetime=MOVE;
   worktime=WORK;
  }

 if (caunter==movetime) /* <-- Ellenfelek mozgatasa */
  {
   int d;
   movetime+=MOVE;
   if (right==1) Moveright();
   else          Moveleft();

   if (destroyed!=1) // <-- Ha a jatekos urhajojat nem erte talalat
    {
     for (d=53; d>-1; d--)
      {
       if (cel[d].ey>370 && cel[d].eactive==1) // <-- Pozicio ellenorzese
        {
         caunter=Game_Over();
         break;
        }
         // Invaders locator, a jatekos helyenek bemerese es loves
       if (cel[d].eactive!=0 && cel[d].ex+22 >= xcp+10 && cel[d].ex+22 <= xcp+33)
        {
         elemp = (BULLET*)  malloc(sizeof(BULLET));
         EinitElem(elemp,d);
         Insert(&lista,elemp);
         break;
        }
      }

      felso = 53;
      falso = 40;
      a1 = random(felso-falso+1)+falso;
      /* Loves talalomra */
      if (cel[a1].eactive && cel[a1+9].eactive!=1 && a1!=d)
       {
        elemp = (BULLET*)  malloc(sizeof(BULLET));
        EinitElem(elemp,a1);
        Insert(&lista,elemp);
        /*
        printf("a1:%d",a1);
        printf("d:%d",d);

        getch();
        */
       }
    }

  Redraw();   // <-- Csillagok ujrarajzolasa
 }

  if (caunter>=worktime)  // <-- Kezdetleges animacio
   {
    worktime+=WORK;

    if (flip==1) { dcolor=9; hcolor=0; flip=0; }
    else         { dcolor=0; hcolor=9; flip=1; }

    for (int iw=0; iw<54; iw++)
     {
      if (cel[iw].eactive==0) continue;

      setcolor(dcolor);
      // jobb
      line(cel[iw].ex+1+27, cel[iw].ey+2+17, cel[iw].ex+3+27, cel[iw].ey+2+17);
      line(cel[iw].ex+4+27, cel[iw].ey+2+17, cel[iw].ex+4+27, cel[iw].ey+5+17);

      // bal
      line(cel[iw].ex-9+27, cel[iw].ey+2+17, cel[iw].ex-12+27, cel[iw].ey+2+17);
      line(cel[iw].ex-12+27, cel[iw].ey+2+17, cel[iw].ex-12+27, cel[iw].ey+5+17);


      setcolor(hcolor);
      // jobb
      line(cel[iw].ex+2+27, cel[iw].ey+1+17, cel[iw].ex+4+27, cel[iw].ey+1+17);
      line(cel[iw].ex+5+27, cel[iw].ey+1+17, cel[iw].ex+5+27, cel[iw].ey+4+17);

      // bal
      line(cel[iw].ex-10+27, cel[iw].ey+1+17, cel[iw].ex-13+27, cel[iw].ey+1+17);
      line(cel[iw].ex-13+27,cel[iw].ey+2+17, cel[iw].ex-13+27,cel[iw].ey+4+17);
     }
   }


   if (caunter>=time)
    {
     time+=SHOOT;
     Bullet_Server(&lista);  // <-- Bullet Server hivasa
    }

    // Altalanos ellenorzesek
   if (hits==54)       // <-- A jatekos gyozott
    {
     setcolor(15); settextstyle(0,0,2);
     outtextxy(210,210,"You Won!");
     delay(1300);
     caunter=899;
    }

   if (destroyed==1)  // <-- A jatekos urhajojat eltalaltak
    {
     if (lives==0) caunter=Game_Over(); // <-- A jatekos vaszit
     leh1++;
     if (leh1>=400)
      {
       destroyed=0; leh1=0;
       putimage(xcp, ycp, player_ship, COPY_PUT);
      }
    }

   if (war==0)           // <-- Nem lehet tuzelni
    {
     leh++;
     if (leh==140) war=1;
    }


   if (kbhit()) // <-- Leutottek egy billentyut
    {
//     Key(Ujgetch());
     ch=Ujgetch();
//     printf("ch: %d",ch);
     if (ch==27) caunter=Game_Over();
     if (ch==13) getch();

     if (ch==77 && destroyed!=1) {   // JOBBRA
       xcp+=7;
       putimage(xcp, ycp ,player_ship,COPY_PUT);
     }

     if (ch==75 && destroyed!=1) {   // BALRA
       xcp-=7;
       putimage(xcp, ycp ,player_ship,COPY_PUT);
     }

     if (ch==32 && war!=0 && destroyed!=1) {   // TUZ
        elemp = (BULLET*)  malloc(sizeof(BULLET));
        InitElem(elemp);
        Insert(&lista,elemp);
        war=0; leh=0;
     }




    }
  }

  // Memoria felszabaditasa

  free(player_ship);
  free(enemy_ship);
  free(live_icon);
  Bullet_Killer(&lista);
  return;
}    // Enf of Game










void Moveright() // <-- Enemy leptetese jobbra
{
 for (int ir=0; ir<54; ir++)
  {
   if (ir==27) Bullet_Server(&lista);
   if (cel[53].ex < 480)
    {
     cel[ir].ex+=7;
     if (cel[ir].eactive==1)
      {
       putimage(cel[ir].ex, cel[ir].ey, enemy_ship, COPY_PUT);

//     if (kbhit()) Key(Ujgetch());
      }
    }
   else
    {
     right=0;
     Stepdown();
     return;
    }
  }

}

void Moveleft() // <-- Enemy leptetese balra
{
 setfillstyle(1,0);
 for (int il=0; il<54; il++)
  {
   if (il==27) Bullet_Server(&lista);
   if (cel[53].ex > 330)
   {
    cel[il].ex-=7;
    if (cel[il].eactive==1)
     {
      putimage(cel[il].ex, cel[il].ey, enemy_ship, COPY_PUT);

//      if (kbhit()) Key(Ujgetch());
     }
   }
   else {
         right=1;
         Stepdown();
         return;
        }

  }
}


void Stepdown()  // <-- Enemy leptetese lefele
{
 setfillstyle(1,0);
 for (int irr=0; irr<54; irr++)
  {
   cel[irr].ey+=7;
   if (cel[irr].eactive==1)
    {
     bar(cel[irr].ex+10,cel[irr].ey-7, cel[irr].ex+36, cel[irr].ey+2);

     putimage(cel[irr].ex, cel[irr].ey, enemy_ship, COPY_PUT);

    }
  }
}

int Ujgetch(void)
{
 int c;

 c=getch();
 if (c==0)
  {
   c=getch()+256;
  }
  return c;
}



void Bullet_Killer(LISTAFEJ* lista)
{
 BULLET* aktualis;
 BULLET* kovetkezo;

 if (lista==NULL) return;
 if (lista->ures) return;

 aktualis=lista->startp;
 kovetkezo=aktualis;

 while (aktualis!=NULL)
  {
   kovetkezo=aktualis->nextp;
   free(aktualis);
   aktualis=kovetkezo;
  }
}

void Bullet_Server(LISTAFEJ* lista)
{
 BULLET* elozo;
 BULLET* aktualis;
 BULLET* kovetkezo;
 int gcolor;

 if (lista == NULL) return; // None bullet

 setfillstyle(1,0);
 aktualis=lista->startp;

 while (aktualis!=NULL)
  {
   if (aktualis->sactive!=0 && aktualis->who==1) // Player's bullet
    {
     setcolor(15); line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy-5);
     setcolor(0);  line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy+5);
     aktualis->sy-=5;

     if (aktualis->sy <=6)
      {
       aktualis->sactive=0;
       setcolor(0);
       line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy+5);
      }

      for (int p=0; p<54; p++)
       {
        if (cel[p].eactive!=0 && aktualis->sactive!=0
        &&  aktualis->sy >= cel[p].ey    && aktualis->sy <= cel[p].ey+20
        &&  aktualis->sx >= cel[p].ex+10 && aktualis->sx <= cel[p].ex+35)
        {
         cel[p].eactive=0;
         bar(cel[p].ex+10, cel[p].ey, cel[p].ex+36, cel[p].ey+23);
         aktualis->sactive=0;
         setcolor(0);
         line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy+5);
         hits++;
         Score(hits);
        }
       }
    }

   if (aktualis->sactive!=0 && aktualis->who==2)  // <-- Enemy's bullet
    {
     setcolor(0); line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy-5);

     for (int g=0; g<6; g++)
      {
       int x,y;
       gcolor=getpixel(aktualis->sx, aktualis->sy+g);

       if (gcolor==ROCKCOLOR)
        {
         x=aktualis->sx;
         y=aktualis->sy+g;
         putpixel(x,y,12);
         aktualis->sactive=0;
         setfillstyle(1,0);
         bar(x-3, y-3, x+3, y+3);
         g=5;
        }
      }

      if (aktualis->sactive!=0)
       {
        setcolor(11); line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy+5);
        aktualis->sy +=5;
       }

      if (aktualis->sy > 500) aktualis->sactive=0;

      if (destroyed!=1 && aktualis->sactive!=0
      && aktualis->sy >= ycp && aktualis->sy <= 464
      && aktualis->sx >= xcp+10 && aktualis->sx <= xcp+32)
       {
        aktualis->sactive=0;
        setcolor(0);
        line(aktualis->sx, aktualis->sy, aktualis->sx, aktualis->sy-5);
        bar(xcp+8, ycp+2, xcp+34, ycp+31);
        destroyed=1;
        lives--;

        switch(lives)
         {
          case 2: putimage(601, 415, live_icon, XOR_PUT);
                  break;
          case 1: putimage(579, 415, live_icon, XOR_PUT);
                  break;
          case 0: putimage(557, 415, live_icon, XOR_PUT);
         }
       }
    }
   aktualis=aktualis->nextp;
  }

  // Dinamikus memoria felszabaditas, ha bullet nem aktiv
  elozo=NULL;
  aktualis=lista->startp;
  kovetkezo=aktualis->nextp;

  while (aktualis!=NULL)
   {
    if (aktualis->sactive==0 && elozo!=NULL && kovetkezo!=NULL)
     {
      elozo->nextp=kovetkezo;
      free(aktualis);
      aktualis=kovetkezo;
      kovetkezo=aktualis->nextp;
     }
    else
     {
      elozo=aktualis;
      aktualis=aktualis->nextp;
      kovetkezo=aktualis->nextp;
     }
   }

}



void Init_Enemy() /* <-- Az ellenseg inicializalasa es kirajzolasa */
{
 int posx = 30;
 int posy = 30;
 int enpos = 0;

 for (int j=0; j<6; j++)
 {
  for (int i=0; i<9; i++)
  {
   posx+=40;
   cel[enpos].ex = posx;
   cel[enpos].ey = posy;
   cel[enpos].eactive = 1;

   putimage(cel[enpos].ex, cel[enpos].ey, enemy_ship, COPY_PUT);
   enpos++;
  }
  posx=30;
  posy+=30;
 }




}






void Graphinit()
{
 clrscr();
 int gdriver = VGA, gmode = VGAHI, errorcode;
 /* Grafikus uzemmod inditasa */
 initgraph(&gdriver,&gmode,"");

 /* Inditas eredmenyenek ellenorzese */
 errorcode = graphresult();
 if (errorcode != grOk) /* <-- Error! */
   {
    printf("Grafikus hiba: %s\n",grapherrormsg(errorcode));
    printf("Press any key to halt:");
    getch();
    exit(1);  /* <-- Kilepes a programbol*/
   }
}

void InitList(LISTAFEJ* fej) /* <-- Lista fejenek inicializalasa */
{
 fej->startp = NULL;
 fej->endp = NULL;
 fej->ures = 1;
}




int Game_Over()
 {
  setcolor(11); settextstyle(0,0,2); setfillstyle(1,0);
  bar(190,205,341,230);
  outtextxy(195,210,"Game Over");
  delay(1300);
  return 899;
 }

void Side()  /* <-- Kepernyo szelenek rajzolasa */
 {
  int kx,ky,iconx;

  kx=getmaxx();
  ky=getmaxy();

  setfillstyle(9,8);
  bar(536,0,kx,ky);
  setcolor(8);
  rectangle(536,0,kx,ky);

  setcolor(8);
   line(530,0,530,kx);
   line(532,0,532,ky);
  setcolor(12);
   line(531,0,531,ky);
  setcolor(8);
  setfillstyle(1,0);
   bar3d(550,350,624,445,0,0);

   bar3d(550,454,624,470,0,0);


 gotoxy(/*74*/71,24);
 printf("Hits:0");

   /*
  setcolor(7);
  settextstyle(2,0,4);
   outtextxy(555,456,"Score:");
   */
 }

void Redraw()   // <-- Csillagok ujrarajzolasa
{
 for (int a11=0; a11<205; a11++)
  {
   if (getpixel(stx[a11],sty[a11]) == 0) putpixel(stx[a11], sty[a11],3);
  }



}






void Rocks(void)  /* <-- Fedezek rajzolasa */
 {
  int c=ROCKCOLOR,size;
  void *rock;

  setfillstyle(1,c); setcolor(c);
   bar(70,405,120,430);
   bar(80,395,110,430);
   line(70,405,80,395);
   floodfill(78,398,c);
   line(110,395,120,405);
   floodfill(111,402,c);

  setfillstyle(1,0); setcolor(0);
   pieslice(95,432,0,180,11);


   size = imagesize(60,380,130,440);
   rock = malloc(size);
   getimage(60,380,130,440, rock);
   putimage(60,380, rock, XOR_PUT);

   putimage(53,380, rock, XOR_PUT);
   putimage(230,380,rock, COPY_PUT);
   putimage(407,380,rock, COPY_PUT);

  free(rock);
 }


void Insert(LISTAFEJ* lista, BULLET* elem) // <-- Kapcsolas a listara
{
 if (lista==NULL || elem == NULL) return;
 // Ha ures a lista, ez az elem lessz az elso
 if (lista->ures)
  {
   lista->startp=elem;
   lista->endp=elem;
   lista->ures=0;
   return;
  }

  // A lista nem ures...

  lista->endp->nextp = elem;
  lista->endp=elem;




}


void InitElem(BULLET* elem)  // <-- Bullet record kitoltese
{
 elem->nextp=NULL;
 elem->sx=xcp+21;
 elem->sy=ycp-1;
 elem->sactive=1;
 elem->who=1;     // <-- Player's bullet
}


void EinitElem(BULLET* elem, int a)  // <-- Bullet record kitoltese
{
 elem->nextp=NULL;
 elem->sx=cel[a].ex+23;
 elem->sy=cel[a].ey+27;
 elem->sactive=1;
 elem->who=2;   // <-- Enemy's bullet
}



void Stars()
 {
  int x,y;
   randomize;
   for (int jj=0; jj<205; jj++)
    {
     x = random(525);
     y = random(490);
     stx[jj] = x;
     sty[jj] = y;
     putpixel(x, y, 3);
    }

 }


void Score(int hts)
{
 gotoxy(/*74*/71,24);
 printf("Hits:%d",hts);
}



void Read(int where)
{
 typedef struct par {
                     unsigned char c;
                     unsigned char n;
                    } PARTIP;

FILE* fpar;
PARTIP p;
int x,y,kx;

fpar=fopen("mol.dat","rb");
/*
if (where==1)
 {
  kx=getmaxx();
  x=529;
  y=0;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    x++;
    if (p.c==7) putpixel(x,y,p.c);
    if (x==kx) { x=529; y++; }
   }

 }
else
 {
  x=510;
  y=125;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    y++;
    if (p.c==7) putpixel(x,y,p.c);
    if (y==235) {x=125; x--; }
   }
 }
 */

if (where==1)
 {
  x=196;
  y=100;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    y++;
    if (p.c==15)
    putpixel(x,y,p.c);
    if /* (y==235) */(p.c==200) {y=100; x++; }
    if (p.c==255) break;
   }
 }
else
 {

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    if (p.c==255) break;
   }

  x=198;
  y=100;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    y++;
    if (p.c!=0 && p.c!=200 && p.c!=255)
      putpixel(x,y,p.c);
    if /* (y==235) */(p.c==200) {y=100; x++; }
    if (p.c==255) break;
   }

 }


 fclose(fpar);
}







void Read2()
{
 typedef struct par {
                     unsigned char c;
                     unsigned char n;
                    } PARTIP;

FILE* fpar;
PARTIP p;
int x,y,kx;

fpar=fopen("mol1.dat","rb");


  x=50;
  y=169;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {
    y++;
    if (p.c==11)
    putpixel(x,y,p.c);
    if /* (y==235) */(p.c==200) {y=169; x++; }
    if (p.c==255) break;
   }


 fclose(fpar);
 }




void Read3()
{
 typedef struct par {
                     unsigned char c;
                     unsigned char n;
                    } PARTIP;

FILE* fpar;
PARTIP p;
int x,y,kx;

fpar=fopen("mol2.dat","rb");

// getimage(550, 300, 565, 315, live_icon);
  x=550;
  y=300;

  while(fread(&p, sizeof(PARTIP),1,fpar)==1)
   {

    y++;
    if (p.c==15)
    putpixel(x,y,p.c);
    if /* (y==235) */(p.c==200) {y=300; x++; }
    if (p.c==255) break;
   }

// rectangle(550, 300, 565, 315);

 fclose(fpar);
 }

