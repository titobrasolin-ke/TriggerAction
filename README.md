# TriggerAction
## Applicazione pilota Trigger Action
Nei siti individuati saranno istallati sensori che misureranno dati relativi a
condizioni climatiche in ambienti esterni, benessere (qualità dell'aria e
microclima) in ambienti interni e alle prestazioni energetiche delle strutture
considerate.

I dati saranno inviati alla [Smart City Platform (SCP)][1].

### Progetto TriggerAction.ServiceModel

Il progetto TriggerAction.ServiceModel contiene tutti i [DTO][2]
dell'applicazione.

### Progetto TriggerAction.ServiceInterface

Il progetto TriggerAction.ServiceInterface implementa la logica di business ed
i servizi per l'accesso alla base dati.

### Progetto TriggerAction

L'applicazione TriggerAction inoltra periodicamente alla Smart City Platform le
misure ricevute dai dispositivi sul campo e salvate sul database locale.

L'applicazione gira come servizio Windows ed espone alcuni servizi web
*self-hosted* che consentono di verificare lo stato del sistema.

Il progetto è stato realizzato con l'autilio di strumenti *open source*
commerciali e non, tra cui:

 * [TopShelf](https://github.com/Topshelf/Topshelf)
 * [Quartz.NET](https://github.com/quartznet/quartznet)
 * [ServiceStack](https://github.com/ServiceStack/ServiceStack)

L'utilizzo di **TopShelf** consente sia di avviare l'applicazione da riga di
comando, sia di installarla come servizio. Ulteriori informazioni si trovano
alla pagina [Topshelf Command-Line Reference — Topshelf 3.0 documentation][3].

[1]: http://sue.enea.it/product/piattaforma-per-gestione-dei-dati-urbani/
[2]: https://it.wikipedia.org/wiki/Oggetto_di_Trasferimento_Dati
[3]: https://topshelf.readthedocs.io/en/latest/overview/commandline.html
