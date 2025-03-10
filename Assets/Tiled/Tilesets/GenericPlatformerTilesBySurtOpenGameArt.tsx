<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.0" name="Generic Platformer Tiles by Surt OpenGameArt" tilewidth="16" tileheight="16" tilecount="768" columns="16">
 <image source="../../Sprites/tilesets/Tiles_by_Surt_OpenGameArt.png" width="256" height="768"/>
 <tile id="1">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="3">
  <properties>
   <property name="unity:layer" value="Ground"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <properties>
   <property name="IsActive" type="bool" value="false"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:layer" value="Ground"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="7">
  <properties>
   <property name="Draggable" type="bool" value="true"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="8">
  <properties>
   <property name="Breakable" type="bool" value="true"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="9">
  <properties>
   <property name="Electric" type="bool" value="true"/>
   <property name="IsActive" type="bool" value="false"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:layer" value="Ground"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="10">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="11">
  <properties>
   <property name="ButtonWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:IsTrigger" type="bool" value="true"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="12">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="3" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="13">
  <properties>
   <property name="GrowPlant" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
   <property name="unity:layer" value="Ground"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="-4" y="-10" width="24" height="12">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
    <ellipse/>
   </object>
   <object id="3" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="66">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="128"/>
  </objectgroup>
 </tile>
 <tile id="82">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="-120" width="16" height="256"/>
  </objectgroup>
 </tile>
 <tile id="146">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="-112" width="16" height="128"/>
  </objectgroup>
 </tile>
 <tile id="304">
  <properties>
   <property name="unity:IsTrigger" type="bool" value="true"/>
   <property name="unity:layer" value="Holes"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="436">
  <properties>
   <property name="CanTeleportThrough" type="bool" value="true"/>
   <property name="unity:layer" value="SemiWall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="544">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="0" width="0.01" height="0.01">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="545">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="0" width="0.01" height="0.01">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="560">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="0" width="0.01" height="0.01">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="561">
  <properties>
   <property name="IsActive" type="bool" value="true"/>
   <property name="ToggleableWall" type="bool" value="true"/>
   <property name="WallNumber" type="int" value="0"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="0" width="0.01" height="0.01">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="582">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="2" x="-16" y="0" width="32" height="32">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="598">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="2" x="-32" y="-16" width="64" height="64">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="599">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="2" x="-48" y="-32" width="96" height="96">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="614">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="3" x="16" y="16">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <polygon points="0,0 0,-16 -16,-16 -14.3828,-15.9258 -12.8979,-15.7116 -11.9374,-15.4846 -10.6279,-15.0814 -9.36747,-14.5816 -7.99242,-13.866 -6.71603,-13.0495 -5.6466,-12.2131 -4.71094,-11.3594 -4.02796,-10.6204 -3.10435,-9.47864 -2.39898,-8.43172 -1.88839,-7.56157 -1.35938,-6.49609 -0.868182,-5.2428 -0.516482,-4.0897 -0.268051,-2.96548 -0.117555,-2.01809 -0.0578825,-1.40892 -0.0213453,-0.874476"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="615">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="32">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <polygon points="0,0 0,-32 -32,-32 -28.7656,-31.8516 -25.7958,-31.4232 -23.8748,-30.9692 -21.2558,-30.1628 -18.7349,-29.1632 -15.9848,-27.732 -13.4321,-26.099 -11.2932,-24.4262 -9.42188,-22.7188 -8.05592,-21.2408 -6.2087,-18.9573 -4.79796,-16.8634 -3.77678,-15.1231 -2.71876,-12.9922 -1.73636,-10.4856 -1.03296,-8.1794 -0.536102,-5.93096 -0.23511,-4.03618 -0.115765,-2.81784 -0.0426906,-1.74895"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="616">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="16" y="48">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <polygon points="0,0 0,-48 -48,-48 -43.1484,-47.7774 -38.6937,-47.1348 -35.8122,-46.4538 -31.8837,-45.2442 -28.1024,-43.7448 -23.9772,-41.598 -20.1482,-39.1485 -16.9398,-36.6393 -14.1328,-34.0782 -12.0839,-31.8612 -9.31305,-28.4359 -7.19694,-25.2951 -5.66517,-22.6847 -4.07814,-19.4883 -2.60454,-15.7284 -1.54944,-12.2691 -0.804153,-8.89644 -0.352665,-6.05427 -0.173648,-4.22676 -0.0640359,-2.62343"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="633">
  <properties>
   <property name="Playfield" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="11.87" width="16" height="4.25">
    <properties>
     <property name="unity:IsTrigger" type="bool" value="true"/>
     <property name="unity:layer" value="Ignore Raycast"/>
     <property name="unity:tag" value="playfieldCollision"/>
    </properties>
    <polygon points="0,0 16,0 16,4.25 0,4.25"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
