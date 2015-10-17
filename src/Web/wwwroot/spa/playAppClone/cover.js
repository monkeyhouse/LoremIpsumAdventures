// Write your Javascript code.
import {inject} from 'aurelia-framework';  
import {Router} from 'aurelia-router';

@inject(Router)
export class Cover{

    constructor(router){
        this.router = router;

        // use moment to process the date
        // moment().fromNow();

        var storyId = $('#StoryId').val();
        this.url = '/Api/Read/Cover/' + storyId;

    }

    activate(){
        return $.get( this.url )
            .success( response => Object.assign(this, response));
    }

    attached(){
        $(document).one('keypress',
            (e) => {  this.next() })
    }

    next(){
        this.router.navigate('jacket');
    }
}
