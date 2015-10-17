// Write your Javascript code.


export class App{

    configureRouter(config, router){

        config.title = 'Profile';
    
        // config.options.baseUrl = 'http://localhost:58533/Profile/Aurelia/';
        config.map([
            { route: ['','aboutme'], name: 'aboutme', moduleId: 'p/aboutme', nav: true, title:'About Me' },
            { route: 'genres', name: 'genres', moduleId: 'p/genres', nav: true, title:'Favorite Genres' },
            { route: 'stories', name: 'stories', moduleId: 'p/stories', nav: true, title:'Favorite Stories' },
            { route: 'mystories', name: 'mystories', moduleId: 'p/mystories', nav: true, title:'My Stories' }
        ]);

        this.router = router;

    }



}