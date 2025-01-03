import 'package:flutter_application/shared/models/post_model.dart';
import 'package:flutter_application/shared/repositories/post_repository.dart';

class PostController   {
  final PostRepository _postRepository;
  PostController([PostRepository? repository])
      : _postRepository = repository ?? PostRepository();

  Future start() async {
    try {
      var test = await _postRepository.findAll();
      return test;
     
    } catch (e) {
      rethrow;
    }
  }

  Future<List<Post>> findAll() async {
    try {
      return await _postRepository.findAll();
    } catch (e) {
      print(e);
      rethrow;
    }
  }

  Future createPost(Post post) async {
    try {
      await _postRepository.createPost(post);
      await findAll();
    } catch (e) {
      print("ERRO ao criar post$e");
    }
  }

  Future delete(String id) async {
    try {
      await _postRepository.delete(id);
      await findAll();
    } catch (e) {
      print("ERRO ao deletar posts $e");
    }
  }

  Future update(Post post) async {
    try {
      await _postRepository.update(post);
      await findAll();
    } catch (e) {
      print("ERRO ao atualizar post $e");
    }
  }

  Future findById(id) async {
    try {
      return await _postRepository.findById(id);
    } catch (e) {
      rethrow;
    }
  }
}
