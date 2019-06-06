import { Component, Inject, Input, OnChanges, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "result-list",
  templateUrl: "./result-list.component.html",
  styleUrls: ["./result-list.component.css"]
})

export class ResultListComponent implements OnChanges {
  @Input() quiz: Quiz;
  results: Result[];
  title: string;

  constructor(private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private router: Router) {
    this.results = [];
  }

  //fired when the @Input() property changes : in this case the quiz member
  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes["quiz"] !== "undefined") {
      //retrieve the quiz variable change info
      var change = changes["quiz"];
      //de eerste keer dat deze opgeroepen wordt is de quiz nog undefined (observer nog niet afgelopen op parent control)
      if (!change.isFirstChange()) {
        //execute the http request and retrieve the result
        this.loadData();
      }
    }
  }

  loadData() {
    var url = this.baseUrl + "api/result/All/" + this.quiz.Id;
    this.http.get<Result[]>(url).subscribe(res => {
      this.results = res;
    }, error => console.log(error));
  }

  onCreate() {
    this.router.navigate(["result/create", this.quiz.Id]);
  }

  onEdit(result: Result) {
    this.router.navigate(["result/edit", result.Id]);
  }

  onDelete(result: Result) {
    var url = this.baseUrl + "api/result/" + result.Id;
    this.http.delete(url).subscribe(res => {
      console.log("Result " + result.Id + " has been deleted");
      this.loadData();
    }, error => console.log(error));
  }
}
