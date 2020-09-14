VAR dataError = false

->Introduccion
== Introduccion ==
- A lonely stranger goes through a fairly crowded amusement park ... #pause
  He is unaware of his surroundings, he only listens to the train of his thoughts: #pause
  //Presionar un boton
  STRANGER
  "I have traveled for years looking for my destiny" #pause
  STRANGER
  "I have longed to find what the future holds ..." #pause
   //Pressing a button
   Suddenly and out of a mirage he saw a tent of a seer. #pause
   //Press button
   STRANGER
   "Could that day be today?" #pause
   STRANGER
   I'll try luck #pause
   And enter without thinking # pause
  ->Conver_inicial
  //Pantalla en negro, entra conversacion

 = Conver_inicial 
 #initConversation
  - What brings you here? Good man
         * [I was walking around these parts and I saw this tent]
         So you want to hear your future?
                 * * [Mmmh… It could be]
                 Why so dubious? -> Conver_final
                 * * [Of course I do!] -> Quiero_Saberlo
                        
         * (Quiero_Saberlo) [I want to know my future]
         Oh wow ... You sound very determined. #pause
         Why do I want to know so much? -> Conver_final
 = Conver_final
  
      * [I'm afraid of the future.]
         SEER
         To be honest, would knowing your future really help you? #pause
         STRANGER
         Sure you do! #pause

         * [All my life I have thought that something great awaits me]
        
         -SEER
         What if something horrible awaits you? #pause
         STRANGER
         I would try to change it. #pause
         SEER
         Boy, can the future really be changed? #pause
         STRANGER
         Let's try it #pause
         SEER
         Mmmh… I need you to answer some questions #pause
         -> Pregunta_Nacimiento

= Pregunta_Nacimiento
  SEER
   {When were you born? | Oh, were you really born in that year? | Wow ... Are you kidding me?} #fecha 
  {     
        -dataError: ->Pregunta_Nacimiento
        -else: ->Pregunta_Ciudad
  }

= Pregunta_Ciudad
  SEER
   {And now tell me, where do you come from? | Are you sure this place exists? | Lying to me can have consequences! }#ciudad
  {     
        -dataError: ->Pregunta_Ciudad
        -else: ->Eleccion_Final
  }

= Eleccion_Final
    STRANGER
-  Anything else? #pause
   SEER
- Let's see boy, choose between one of these two cards. #pause
-  #initPuzzle
   -> END

        