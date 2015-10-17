
export class Genres{

    constructor(){

        this.urls ={
            base : '/Api/FavoriteGenres/ ',
            getAllGenres :'/Api/FavoriteGenres/GetAllGenres',
            add : '/Api/FavoriteGenres/Add',
            remove :'/Api/FavoriteGenres/Remove'
        };

        this.filterOpts = [
            { text : 'and include favorites ', value : 0 } ,
            { text : 'and exclude favorites', value : 1 } ,
            { text : 'with favorites only ', value : 2 } 
        ];

        this._selectedOpt = this.filterOpts[0];

        this._searchText = '';

        this.genres = [];

    }

    
    activate(){       
        //debugger;

        //api
        // Profile/FavoriteGenre
        // add ( genreId )
        // remove ( genreId )
        // getAll() //favorites!
        // getAllGenres
        
        var url = '/ApiFavoriteGenres/GetAllGenres';
        return $.get(this.urls.getAllGenres).then(response => {
            //var content = response.content;
            var genres = [];
            response.forEach( t => genres.push( new FavoriteGenre(t)));
            this.allGenres = genres;
            this.genres = genres.slice(0);
        });

    }
	


    
    get selectedOpt(){
        return this._selectedOpt;
    }

    set selectedOpt( value ){
        this._selectedOpt = value;
        this.execFilter();
    }

    get searchText(){
        return this._searchText;
    }

    set searchText(value){
        this._searchText = value;
        this.execFilter();
    }
    
    favorite(g){

        //console.log(g);
        var url = g.isFav ? this.urls.remove : this.urls.add;

        g.isFav = !g.isFav;

        $.post( this.urls.add )
         .error( (jqXHR) => {
             // show error notice
             Up.ErrorNotice(jqXHR);
             //revert state

             g.isFav = !g.isFav;
            
         });

    }

    execFilter(){			
        var text = this._searchText;
        var favOpt = this._selectedOpt.value;

        var all = this.allGenres.filter( 
               r => r.name.includesI( text) | r.desc.includesI( text ) 
               );
        

        if ( favOpt == 1 ) {
            //no favorites
            all = all.filter( r => !r.isFav );
        }
        if ( favOpt == 2 ) {
            //only favorites
            all = all.filter( r => r.isFav );
        }

        this.genres = all;


    }	
}

export class FavoriteGenre{
    constructor(data){
        this.name = '';
        this.desc = '';
        this.isFav = false;

        Object.assign(this, data)
    }
}

