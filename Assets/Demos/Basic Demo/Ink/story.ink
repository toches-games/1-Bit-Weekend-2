VAR dataError = false

->Introduccion
== Introduccion ==
- Un hombre forastero y solitario va por un parque de atracciones bastante concurrido... #pause
  Él no se da cuenta de su alrededor, solo hace caso al hilo de sus pensamientos: #pause
  //Presionar un boton
  "He viajado por años buscando mi destino, he anhelado poder encontrar que me depara el futuro…"#pause
  //Presionar un boton
  De pronto y como salida de un espejismo vió una carpa de un vidente. #pause
  //Presionar boton
  "¿Ese día podría ser hoy? Probaré suerte." #pause
  Y entra sin pensarlo.#pause
  ->Conver_inicial
  //Pantalla en negro, entra conversacion

 = Conver_inicial 
 #initConversation
  - VIDENTE
  ¿Qué te trae por aquí? Buen hombre 
        *[Caminaba por estos lares y vi esta carpa]
        ¿Entonces quieres escuchar tu futuro?
                * *[Mmmh… Puede ser]
                ¿Por qué tan dudoso? ->Conver_final
                * *[¡Por supuesto que sí!] -> Quiero_Saberlo
                        
        *(Quiero_Saberlo) [Quiero saber mi futuro]
        Oh vaya… Se te escucha muy decidido. #pause
        ¿Por qué tanto deseo en saberlo? ->Conver_final 

 = Conver_final
  
        *[Le tengo miedo al futuro.]
        Siendo sincero, ¿saber tu futuro te ayudaría realmente?
        ¡Claro que sí!

        *[Toda mi vida he pensado que me espera algo grande]
        
        -¿Y si te espera algo horrible? #pause
        Trataría de cambiarlo. #pause
        Muchacho, ¿El futuro realmente puede cambiarse? #pause
        Vamos a probarlo #pause
        ->Pregunta_Nacimiento

= Pregunta_Nacimiento
  {Necesito que respondas a algunas preguntas antes. ¿En qué fecha naciste? | Oh, tu fecha de nacimiento es erronea, por favor vuelve a intentarlo | Vaya... ¿Me estas tomando el pelo?} #fecha 
  {     
        -dataError: ->Pregunta_Nacimiento
        -else: ->Pregunta_Ciudad
  }

= Pregunta_Ciudad
  {Y ahora dime, ¿De dónde vienes? | Ehm... ¿Estas seguro que existe este lugar? | ¡Mentirme puede traer consecuencias! }#ciudad
  {     
        -dataError: ->Pregunta_Ciudad
        -else: ->Eleccion_Final
  }

= Eleccion_Final
-  ¿Necesita saber algo más? #pause
-  Pues si, en realidad si. Elige entre una de estas dos cartas. #pause 
-  #initPuzzle
   -> END

        