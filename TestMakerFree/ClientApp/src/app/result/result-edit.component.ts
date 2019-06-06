import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { error } from "protractor";

@Component({
  selector: "result-edit",
  templateUrl: "./result-edit.component.html",
  styleUrls: ["./result-edit.component.css"]
})

export class ResultEditComponent {
  title: string;
  result: Result;
  //this item will be TRUE when editing an existing result, FALSE when creating a new one
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private base_url: string) {
    //create an empty object from the Quiz interface
    this.result = <Result>{};

    //a plus before a variable returns its number representation!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    var id = +this.activatedRoute.snapshot.params["id"];

    //check if we're in edit-mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //fetch the result from the server: in this case the id is the resultId
      var url = this.base_url + "api/result/" + id;
      this.http.get<Result>(url).subscribe(res => {
        this.result = res;
        this.title = "Edit - " + this.result.Text;
      }, error => console.log(error))
    }
    else {
      //create new result for the Quiz: in this case the id is the quizId
      this.result.QuizId = id;
      this.title = "Create a new Result";
    }
  }

  onSubmit(result: Result){
    var url = this.base_url + "api/result";
    if (this.editMode) {
      this.http.post<Result>(url, result).subscribe(res => {
        var v = res;
        console.log("Result " + v.Id + " has been updated.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
    else {
      this.http.put<Result>(url, result).subscribe(res => {
        var v = res;
        console.log("Result " + v.Id + " has been created");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.result.QuizId]);
  }

}
