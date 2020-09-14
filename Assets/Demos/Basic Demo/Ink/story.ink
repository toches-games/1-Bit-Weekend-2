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
  - ¿Qué te trae por aquí? Buen hombre 
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
        VIDENTE
        Siendo sincero, ¿saber tu futuro te ayudaría realmente? #pause
        FORASTERO
        ¡Claro que sí!#pause

        *[Toda mi vida he pensado que me espera algo grande]
        
        -VIDENTE
        ¿Y si te espera algo horrible? #pause
        FORASTERO
        Trataría de cambiarlo. #pause
        VIDENTE
        Muchacho, ¿El futuro realmente puede cambiarse? #pause
        FORASTERO
        Vamos a probarlo #pause
        VIDENTE
        Mmmh… Necesito que respondas algunas preguntas #pause
        ->Pregunta_Nacimiento

= Pregunta_Nacimiento
  VIDENTE
  {¿En qué fecha naciste? | Oh, ¿de verdad naciste en ese año? | Vaya... ¿Me estas tomando el pelo?} #fecha 
  {     
        -dataError: ->Pregunta_Nacimiento
        -else: ->Pregunta_Ciudad
  }

= Pregunta_Ciudad
  VIDENTE
  {Y ahora dime, ¿De dónde vienes? | ¿Estas seguro que existe este lugar? | ¡Mentirme puede traer consecuencias! }#ciudad
  {     
        -dataError: ->Pregunta_Ciudad
        -else: ->Eleccion_Final
  }

= Eleccion_Final
    FORASTERO
-  ¿Algo más? #pause
  VIDENTE
-  A ver muchacho, elige entre una de estas dos cartas. #pause 
-  #initPuzzle
   -> END

        