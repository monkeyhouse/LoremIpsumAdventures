// Write your Javascript code.


export class App{

    configureRouter(config, router){


        var title = 'Lorem Ipsum Express';

        var fTitle = (x) => { x == null ? title : title + ' | ' + x } ;
    
        config.map([
            { route: ['','cover'], name: 'cover', moduleId: 'p/cover', nav: true, title: fTitle('Cover') },
            { route: 'jacket', name: 'jacket', moduleId: 'p/jacket', title: fTitle('Jacket') },
            { route: 'page/:pageId', name: 'page', moduleId: 'p/page', title: fTitle('Reading In Progress') },
        ]);

        this.router = router;

    }



}