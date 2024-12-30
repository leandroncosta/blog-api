class Post {
  String id;
  String userId;
  String title;
  String content;
  String date;

  Post(
      {required this.id,
      required this.userId,
      required this.title,
      required this.content,
      required this.date});

  factory Post.fromJson(Map<String, dynamic> json) {
    return Post(
        id: json['id'],
        userId: json['userId'],
        title: json['title'],
        content: json['content'],
        date: json['date']
        );
  }

  Map toJson() {
    return {
        id: "",
        userId:"",
        title:"",
        content: "",
        date: ""
    };
  }
}
